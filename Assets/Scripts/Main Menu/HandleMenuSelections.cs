using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class HandleMenuSelections : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    private float optionsCameraPathPos = 1;

    public void MoveCameraAndSwitchToOptionsMenu()
    {
        
    }
    public void MoveCameraAndSwitchToCreditsMenu()
    {
        
    }
    public void MoveCameraAndSwitchToMainMenu()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Application");
        Application.Quit();
    }
}
