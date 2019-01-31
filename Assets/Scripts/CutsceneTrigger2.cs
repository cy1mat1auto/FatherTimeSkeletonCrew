using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneTrigger2 : MonoBehaviour
{
    public GameObject cutScene;
    private bool inCutscene = false;
    PlayerMove movescript;
    public bool cutSceneDone = false;
    //Duration of each frame, in seconds
    public float Duration;
    //An index to keep track of frame time
    public float timing;
    //An index to keep track of cutscene progression
    public int SceneProgress;
    //A bunch of UI Sprites; drag-and-drop is probably the best way to deal with this
    public Sprite im1; public Sprite im2; public Sprite im3; public Sprite im4; public Sprite im5;
    public Sprite im6; public Sprite im7; public Sprite im8; public Sprite im9; public Sprite im10;
    //A value, in case your cutscene does not fill this whole script. This should default to the maximum length of the script;
    public int SceneLength = 10;
    //This part allows us to create a list of the UI sprites used to generate the cutscene, allowing us to iterate through the scenes in order
    public List<Sprite> BufferStack;
    public List<Sprite> SceneStack;

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
        }
    }

    public void Update()
    {
        // Controls frame timing;
        if (Time.unscaledTime - timing >= Duration && SceneProgress < SceneStack.Count - 1)
        {
            timing = Time.unscaledTime;
            SceneProgress += 1;
            cutScene.GetComponent<Image>().sprite = SceneStack[SceneProgress];
        }

        else if (Time.unscaledTime - timing >= Duration && SceneProgress == SceneStack.Count -1)
        {
            cutSceneDone = true;
        }

        // exits cutscene if player presses correct exit key(s)
        if ((inCutscene && Input.GetKeyDown(KeyCode.Escape)) || cutSceneDone)
        {
            inCutscene = false;
            Time.timeScale = 1;
            cutScene.SetActive(false);
            movescript.canMove = true;
            cutSceneDone = false;
            Destroy(GetComponentInParent<Collider>());
        }
    }
}
