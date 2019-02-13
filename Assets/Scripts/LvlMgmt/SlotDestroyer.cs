using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlotDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("DontDestroyOnLoad"));
        Debug.Log("Slot Destroyed");
    }

}