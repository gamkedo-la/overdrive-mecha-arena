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

    FMOD.Studio.Bus MasterBus;
    FMOD.Studio.Bus MusicBus;
    FMOD.Studio.Bus SoundsBus;

    float MasterBusVol = 1.0f;
    float MusicBusVol = 1.0f;
    float SoundsBusVol = 1.0f;

    private void Awake()
    {
        fmodMenuEvent = "event:/MenuTest";
    }

    private void Start()
    {
        menuState = RuntimeManager.CreateInstance(fmodMenuEvent);
        menuState.start();

        //FMOD Layer ID Caches
        #region
        FMOD.Studio.EventDescription mainEventDescription;
        menuState.getDescription(out mainEventDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION mainParameterDescription;
        mainEventDescription.getParameterDescriptionByName("Main Menu Layer On", out mainParameterDescription);
        mainLayerID = mainParameterDescription.id;

        FMOD.Studio.EventDescription optionsEventDescription;
        menuState.getDescription(out optionsEventDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION optionsParameterDescription;
        optionsEventDescription.getParameterDescriptionByName("Options Layer On", out optionsParameterDescription);
        optionsLayerID = optionsParameterDescription.id;

        FMOD.Studio.EventDescription creditsEventDescription;
        menuState.getDescription(out creditsEventDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION creditsParameterDescription;
        creditsEventDescription.getParameterDescriptionByName("Credits Layer On", out creditsParameterDescription);
        creditsLayerID = creditsParameterDescription.id;
        #endregion

        menuState.setParameterByID(mainLayerID, 1f);
    }

    public void SwitchToOptionsLayer()
    {
        menuState.setParameterByID(optionsLayerID, 1f);
    }

    public void SwitchToCreditsLayer()
    {
        menuState.setParameterByID(creditsLayerID, 1f);
    }

    public void SwitchMainMenuLayer()
    {
        menuState.setParameterByID(optionsLayerID, 0f);
        menuState.setParameterByID(creditsLayerID, 0f);
        menuState.setParameterByID(mainLayerID, 1f);
    }

    public void StopMainMenuMusic()
    {
        menuState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void ChangeMusicVolume()
    {
        //Lower/Increase music bus volume according to slider
        //save bus volume so it carries over to other scenes
    }
    public void ChangeSoundsVolume()
    {
        //Lower/Increase sounds bus volume according to slider
        //save bus volume so it carries over to other scenes
    }
    public void ChangeMasterVolume()
    {
        //Lower/Increase master bus volume according to slider
        //save bus volume so it carries over to other scenes
    }
}
