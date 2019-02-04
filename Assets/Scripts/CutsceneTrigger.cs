using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public GameObject cutScene = null;
    private GameObject cutsceneClone;
    private bool inCutscene = false;
    PlayerMove movescript;
    public bool cutSceneDone = false; 

    private void OnTriggerEnter(Collider other)
    { 
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger");
            Time.timeScale = 0;

            // Freezes player movement
            movescript = other.GetComponent<PlayerMove>();
            movescript.canMove = false;

            // instantiates new cutscene object
            cutsceneClone = Instantiate(cutScene, transform.position, Quaternion.identity);

            inCutscene = true;
        }
    }

    public void Update()
    {
        // exits cutscene if player presses correct exit key(s)
        if ((inCutscene && Input.GetKeyDown(KeyCode.Escape)) || cutSceneDone)
        {
            inCutscene = false;
            Time.timeScale = 1;
            Destroy(cutsceneClone);
            movescript.canMove = true;
            cutSceneDone = false;
        }
    }
}
