using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneTrigger3 : MonoBehaviour
{
    public GameObject cutScene;
    private bool inCutscene = false;
    PlayerMove movescript;
    public bool cutSceneDone = false;
    // Duration of each frame, in seconds if individual Duration is empty for 
    // Particular frame. Defualts to 5 seconds
    public int DefaultDuration = 5;
    // If user enters invalid DefaultDuration corrects to valid number
    private readonly int FallBackDuration = 5;
    // Individual duration of each frame, each csv is a new frame length 
    public string IndividualDuration;
    //An index to keep track of frame time
    private float timing;
    //An index to keep track of cutscene progression
    private int SceneProgress;
    //A bunch of UI Sprites; drag-and-drop is probably the best way to deal with this
    public Sprite im1; public Sprite im2; public Sprite im3; public Sprite im4; public Sprite im5;
    public Sprite im6; public Sprite im7; public Sprite im8; public Sprite im9; public Sprite im10;
    //A value, in case your cutscene does not fill this whole script. This should default to the maximum length of the script;
    public int SceneLength = 10;
    //This part allows us to create a list of the UI sprites used to generate the cutscene, allowing us to iterate through the scenes in order
    public List<Sprite> BufferStack;
    public List<Sprite> SceneStack;
    // This is a list of duration of each frame generated from IndividualDuration 
    // and DefualtDuration inputs
    public List<int> Duration;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Sprite[] input = { im1, im2, im3, im4, im5, im6, im7, im8, im9, im10 };
            BufferStack.AddRange(input);
            SceneStack.AddRange(BufferStack.GetRange(0, SceneLength));
            Debug.Log(SceneStack.Count);
            Time.timeScale = 0;
            timing = Time.unscaledTime;

            // Freezes player movement
            movescript = other.GetComponent<PlayerMove>();
            movescript.canMove = false;

            // instantiates new cutscene object
            cutScene.SetActive(true);
            SceneProgress = 0;
            cutScene.GetComponent<Image>().sprite = SceneStack[0];

            inCutscene = true;

            // Creates a list of length SceneLength with the duration for each 
            // frame at same index as frame in SceneStack
            GenerateDuration();
        }
    }

    public void Update()
    {
        if (inCutscene)
        {
            // Controls frame timing;
            if (Time.unscaledTime - timing >= Duration[SceneProgress] && SceneProgress < SceneStack.Count - 1)
            {
                timing = Time.unscaledTime;
                SceneProgress += 1;
                cutScene.GetComponent<Image>().sprite = SceneStack[SceneProgress];
            }

            else if (Time.unscaledTime - timing >= Duration[SceneProgress] && SceneProgress == SceneStack.Count - 1)
            {
                cutSceneDone = true;
            }

            // exits cutscene if player presses correct exit key(s)
            if (Input.GetKeyDown(KeyCode.E) || cutSceneDone)
            {
                ExitCutscene();

            }

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneProgress += 1;
                if (SceneProgress < SceneStack.Count)
                {
                    cutScene.GetComponent<Image>().sprite = SceneStack[SceneProgress];
                }

                else
                {
                    ExitCutscene();
                }

            }
        }
    }

    private void ExitCutscene()
    {
        inCutscene = false;
        Time.timeScale = 1;
        cutScene.SetActive(false);
        movescript.canMove = true;
        cutSceneDone = false;
        Destroy(GetComponentInParent<Collider>());
    }

    private void GenerateDuration()
    {
        string[] tempDuration = IndividualDuration.Split(',');
        int lengthTempDur = tempDuration.Length;

        // Loops SceneLength times to generate list of individual frames
        for (int i = 0; i < SceneLength; i++)
        {
            // If value in tempDuration is a valid number greater than 0 add it 
            // to Duration
            if (i < lengthTempDur && int.TryParse(tempDuration[i], out int dur) && dur > 0)
            {
                Duration.Add(dur);
            }

            // If value in tempDuration is invalid or we are outside tempDuration 
            // add Defualt value if its positive
            else if (DefaultDuration > 0)
            {
                Duration.Add(DefaultDuration);
            }

            else
            {
                Duration.Add(FallBackDuration);
            }
        }
    }
}
