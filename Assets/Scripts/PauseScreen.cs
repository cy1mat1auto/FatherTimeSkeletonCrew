using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    //This version of the script should work when attached to the player object (the Jockey spaceship)
    public bool InCutScene = false;
    public bool paused = false;
    public GameObject screen = null;
    public GameObject Main, UGScreen, SettingScreen;
    public Button resume, save, quit, settings, upgrades, UGBack, SettingsBack;
    public Toggle SettingsInvert;
    public Slider Sensitivity;
    //the next two floats are used to asynchronously load the main menu. Read on for more details.
    private float timer, timeofquit = 100000000f;
    private Coroutine Quitter;

    // Start is called before the first frame update
    void Start()
    {
        //Upon Start, automatically assign buttons. This makes the prefabs easier to use.
        if (screen == null)
        {
            screen = GameObject.FindGameObjectWithTag("PauseScreen");
        }

        if (resume == null)
        {
            resume = screen.transform.Find("PauseMain/ResumeButton").GetComponent<Button>();
        }

        if (quit == null)
        {
            quit = screen.transform.Find("PauseMain/QuitButton").GetComponent<Button>();
        }

        //if (save == null)
        //{
        //    save = screen.transform.Find("PauseMain/SaveButton").GetComponent<Button>();
        //}

        if (upgrades == null)
        {
            upgrades = screen.transform.Find("PauseMain/UpgradeButton").GetComponent<Button>();
        }

        if (UGBack == null)
        {
            UGBack = screen.transform.Find("Upgrades/BackButton").GetComponent<Button>();
        }

        if (settings == null)
        {
            settings = screen.transform.Find("PauseMain/SettingsButton").GetComponent<Button>();
        }

        if (SettingsBack == null)
        {
            SettingsBack = screen.transform.Find("Settings/BackButton").GetComponent<Button>();
        }

        if (SettingsInvert == null)
        {
            SettingsInvert = screen.transform.Find("Settings/InvertedToggle").GetComponent<Toggle>();
        }

        if (Main == null)
        {
            Main = screen.transform.Find("PauseMain").gameObject;
        }

        if (UGScreen == null)
        {
            UGScreen = screen.transform.Find("Upgrades").gameObject;
        }

        if (SettingScreen == null)
        {
            SettingScreen = screen.transform.Find("Settings").gameObject;
        }

        if (Sensitivity == null)
        {
            Sensitivity = screen.transform.Find("Settings/SensitivitySlider").GetComponent<Slider>();
        }

        screen.SetActive(false);
        Main.SetActive(true);
        UGScreen.SetActive(false);
        SettingScreen.SetActive(false);
        SettingsInvert.isOn = gameObject.GetComponent<PlayerMove>().Inverted;
        Sensitivity.value = gameObject.GetComponent<PlayerMove>().Sensitivity;

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
            resume.onClick.AddListener(ResumeGame);
            quit.onClick.AddListener(MainMenu);
            upgrades.onClick.AddListener(GoToUpgrades);
            UGBack.onClick.AddListener(GoToMain);
            settings.onClick.AddListener(GoToSettings);
            SettingsBack.onClick.AddListener(GoToMain);

            SettingsInvert.onValueChanged.AddListener(delegate { Invert(SettingsInvert); });
            Sensitivity.onValueChanged.AddListener(delegate { XYSensitivity(Sensitivity); });
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
        if (InCutScene)
        {
            paused = false;
            screen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }

        else
        {
            paused = false;
            Time.timeScale = 1;
            GetComponent<PlayerMove>().canMove = true;
            screen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }

    void MainMenu()
    {
        timeofquit = Time.unscaledTime;
        //SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    void GoToUpgrades()
    {
        Main.SetActive(false);
        UGScreen.SetActive(true);
        SettingScreen.SetActive(false);
    }

    void GoToSettings()
    {
        Main.SetActive(false);
        UGScreen.SetActive(false);
        SettingScreen.SetActive(true);
    }

    void GoToMain()
    {
        Main.SetActive(true);
        UGScreen.SetActive(false);
        SettingScreen.SetActive(false);
    }

    void Invert(Toggle change)
    {
        gameObject.GetComponent<PlayerMove>().Inverted = SettingsInvert.isOn;
    }

    void XYSensitivity(Slider change)
    {
        gameObject.GetComponent<PlayerMove>().Sensitivity = Sensitivity.value;
    }
}
