using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    //This version of the script should work when attached to the player object (the Jockey spaceship)
    public bool paused = false;
    public GameObject screen;
    public Button resume, save, quit;
    //the next two floats are used to asynchronously load the main menu. Read on for more details.
    private float timer, timeofquit = 100000000f;
    private Coroutine Quitter;

    // Start is called before the first frame update
    void Start()
    {
        screen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused && Input.GetKeyDown(KeyCode.Escape))
        {
            paused = true;
            Debug.Log("paused");
            Time.timeScale = 0;
            GetComponent<PlayerMove>().canMove = false;
            screen.SetActive(true);
        }

        //if (paused && Input.GetKeyUp(KeyCode.Escape))
        //{
            //ResumeGame();
        //}

        if (paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Quitter = StartCoroutine(LoadMenu());
            timer = Time.unscaledTime;
            //for the moment, buttons DO NOT restart in-engine time.
            resume.onClick.AddListener(ResumeGame);
            quit.onClick.AddListener(MainMenu); 
            
        }
    }

    private IEnumerator LoadMenu()
    {
        yield return new WaitForSecondsRealtime(timeofquit - timer);
        AsyncOperation quitting = SceneManager.LoadSceneAsync("MainMenu");
        Debug.Log("You have exited the game");
    }

    void ResumeGame()
    {
        paused = false;
        Time.timeScale = 1;
        GetComponent<PlayerMove>().canMove = true;
        screen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void MainMenu()
    {
        timeofquit = Time.unscaledTime;
        //SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
