using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFMODEventsScript : MonoBehaviour
{
    FMOD.Studio.EventInstance UI_HoverOverButton;
    FMOD.Studio.EventInstance UI_QuitGame;
    FMOD.Studio.EventInstance UI_StartGame;
    FMOD.Studio.EventInstance UI_MenuSelection;
    FMOD.Studio.EventInstance UI_Back;

    private void Awake()
    {
        UI_MenuSelection = FMODUnity.RuntimeManager.CreateInstance("event:/UI_MenuSelection");
        UI_QuitGame = FMODUnity.RuntimeManager.CreateInstance("event:/UI_QuitGame");
        UI_StartGame = FMODUnity.RuntimeManager.CreateInstance("event:/UI_StartGame");
        UI_HoverOverButton = FMODUnity.RuntimeManager.CreateInstance("event:/UI_HoverOverButton");
        UI_Back = FMODUnity.RuntimeManager.CreateInstance("event:/UI_Back");
    }

    public void PlaySoundWhenMouseEnterButton()
    {
        UI_HoverOverButton.start();
    }

    public void PlaySoundWhenQuitButtonPressed()
    {
        UI_QuitGame.start();
    }

    public void PlaySoundWhenStartGamePressed()
    {
        UI_StartGame.start();
    }

    public void PlaySoundWhenPressingButton()
    {
        UI_MenuSelection.start();
    }

    public void PlaySoundWhenBackButtonPressed()
    {
        UI_Back.start();
    }
}
