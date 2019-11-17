using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class HandleMenuSelections : MonoBehaviour
{
    private int sceneIndex = 0;

    [SerializeField] CinemachineVirtualCamera virtualCamera;
    private float optionsCameraPathPos = 1;

    public void MoveCameraAndSwitchToOptionsMenu()
    {
        CinemachineTrackedDolly vcamTrackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        vcamTrackedDolly.m_PathPosition = 1;
    }
    public void MoveCameraAndSwitchToCreditsMenu()
    {
        CinemachineTrackedDolly vcamTrackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        vcamTrackedDolly.m_PathPosition = 2;
    }
    public void MoveCameraAndSwitchToMainMenu()
    {
        CinemachineTrackedDolly vcamTrackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        vcamTrackedDolly.m_PathPosition = 0;
    }

    public void StartGame()
    {
        sceneIndex++;
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Application");
        Application.Quit();
    }
}
