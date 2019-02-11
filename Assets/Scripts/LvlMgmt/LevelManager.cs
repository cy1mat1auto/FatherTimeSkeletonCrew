using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    //Before you begin, make sure the desired scenes are loaded into the build settings.
    //Create Buttons for each level. Use the inspector to drag-and-drop the buttons into the slots.
    public Button level1, level2, level3, level4, level5;
    public string l1, l2, l3, l4, l5;
    public GameObject PreservedSettings;

    //Void Awake will protect PreservedSettings; This is a gameobject containing scripts that hold the player's
    //upgrades, audiovisual and other gameplay settings;
    //private void Awake()
    //{
        //DontDestroyOnLoad(PreservedSettings);
    //}

    //Add a listener to the each button. On click, the listener will then call the SceneManager.
    void Start()
    {
        level1.onClick.AddListener(LoadLevel1);
    }

    // create a void function that calls the scenemanager to load the desired scene associated with each button.
    void LoadLevel1()
    {
        SceneManager.LoadScene(l1, LoadSceneMode.Single);
        DontDestroyOnLoad(PreservedSettings);
        Time.timeScale = 1;
    }

}
