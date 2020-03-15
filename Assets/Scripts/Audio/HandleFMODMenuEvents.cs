using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class HandleFMODMenuEvents : MonoBehaviour
{
    [EventRef]
    private string fmodMenuEvent = "";

    FMOD.Studio.EventInstance menuState;
    FMOD.Studio.PARAMETER_ID mainLayerID, optionsLayerID, creditsLayerID;

    

    private void Awake()
    {
        fmodMenuEvent = "event:/BackgroundMusic";
    }

    private void Start()
    {
        menuState = RuntimeManager.CreateInstance(fmodMenuEvent);
        menuState.start();
    }
}
