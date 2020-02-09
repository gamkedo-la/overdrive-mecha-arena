using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class GameplayFMODEventsScript : MonoBehaviour
{
    [EventRef]
    private string fmodMenuEvent = "";

    FMOD.Studio.EventInstance menuState;
    FMOD.Studio.PARAMETER_ID mainLayerID, optionsLayerID, creditsLayerID;

    private void Awake()
    {
        fmodMenuEvent = "event:/MenuTest";
    }

    void Start()
    {
        menuState = RuntimeManager.CreateInstance(fmodMenuEvent);
        menuState.start();

        // FMOD Layer ID Caches
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
        // End of FMOD Layer ID Caches

        menuState.setParameterByID(optionsLayerID, 1f);
        menuState.setParameterByID(mainLayerID, 1f);
        menuState.setParameterByID(creditsLayerID, 1f);
    }
}
