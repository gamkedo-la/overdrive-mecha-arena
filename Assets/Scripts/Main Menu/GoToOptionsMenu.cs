using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GoToOptionsMenu : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    private float optionsCameraPathPos = 1;

    public void MoveCameraAndSwitchMenus()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Application");
        Application.Quit();
    }
}
