using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public static bool GameIsPaused;
    public GameObject pauseUI;
    public GameObject gameUI;

    private void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            if(GameIsPaused)
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
    }

    public void Quit()
    {
        Application.Quit();
    }
}
