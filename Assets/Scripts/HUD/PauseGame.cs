using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    private GameObject audiomanager;

    public static bool GameIsPaused;
    public GameObject pauseUI;
    public GameObject gameUI;
    FMOD.Studio.Bus MasterBus;

    private UIFMODEventsScript pauseMenuFMODHandler;

    private void Start()
    {
        MasterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
        audiomanager = GameObject.Find("AudioManager");

        if (audiomanager != null)
        {
            pauseMenuFMODHandler = audiomanager.GetComponent<UIFMODEventsScript>();
        }
        else
        {
            Debug.LogWarning("Audio manager is not active. Please active it to enable audio and FMOD events.");
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Back");

            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        if (audiomanager != null && audiomanager.activeSelf == true)
        {
            pauseMenuFMODHandler.StopMusicSnapshot();
        }

        Cursor.lockState = CursorLockMode.Locked;

        pauseUI.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    private void Pause()
    {
        if (audiomanager != null && audiomanager.activeSelf == true)
        {
            pauseMenuFMODHandler.StartMusicFilterSnapshot();
        }

        Cursor.lockState = CursorLockMode.None;

        gameUI.SetActive(false);
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
