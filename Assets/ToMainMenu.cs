using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMainMenu : MonoBehaviour
{
    FMOD.Studio.Bus masterBus;

    private void Awake()
    {
        masterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
    }
    public void MainMenu()
    {
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene(0);
    }
}
