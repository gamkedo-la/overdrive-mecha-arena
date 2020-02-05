using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{


    GameObject audiomanagerScript;

    public static bool GameIsPaused;
    public GameObject pauseUI;
    public GameObject gameUI;
    FMOD.Studio.Bus MasterBus;

    private void Start()
    {
        MasterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
        audiomanagerScript = GameObject.Find("AudioManager");

    }

    private void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Back");
            
            if(GameIsPaused)
            {
                audiomanagerScript.GetComponent<UIFMODEventsScript>().StopMusicSnapshot();
                Resume();
            }
            else
            {
                audiomanagerScript.GetComponent<UIFMODEventsScript>().StartMusicFilterSnapshot();
                Pause();
            }
        }
    }
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;

        pauseUI.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    private void Pause()
    {
        Cursor.lockState = CursorLockMode.None;

        gameUI.SetActive(false);
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
        MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

    }

    public void Quit()
    {
        Application.Quit();
    }
}
