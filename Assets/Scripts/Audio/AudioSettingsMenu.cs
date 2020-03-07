using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;

public class AudioSettingsMenu : MonoBehaviour
{
    [EventRef]
    private string soundsVolTest;

    FMOD.Studio.Bus MasterBus;
    FMOD.Studio.Bus MusicBus;
    FMOD.Studio.Bus SoundsBus;

    float MasterBusVol = 1.0f;
    float MusicBusVol = 1.0f;
    float SoundsBusVol = 1.0f;

    private GameplayFMODEventsScript gameplayAudio;
    private FinalResultsFMODEvents finalResultsAudio;

    private void Awake()
    {
        gameplayAudio = GetComponent<GameplayFMODEventsScript>();
        finalResultsAudio = GetComponent<FinalResultsFMODEvents>();
    }

    private void Start()
    {
        MasterBus = RuntimeManager.GetBus("bus:/Master");
        MusicBus = RuntimeManager.GetBus("bus:/Master/Music");
        SoundsBus = RuntimeManager.GetBus("bus:/Master/Sounds");

        soundsVolTest = "event:/UI/UI_MenuSelection";

        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            gameplayAudio.enabled = false;
            finalResultsAudio.enabled = false;
        }
        else if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            gameplayAudio.enabled = true;
        }
        else if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            gameplayAudio.enabled = false;
            finalResultsAudio.enabled = true;
        }

        MasterBus.setVolume(MasterBusVol);
        MusicBus.setVolume(MusicBusVol);
        SoundsBus.setVolume(SoundsBusVol);
    }

    public void MasterVolumeLevel(float newMasterVol)
    {
        MasterBusVol = newMasterVol;
    }
    public void MusicVolumeLevel(float newMusicrVol)
    {
        MusicBusVol = newMusicrVol;
    }
    public void SoundsVolumeLevel(float newSoundsVol)
    {
        SoundsBusVol = newSoundsVol;

        // next code block used for testing sounds volume slider changes when we get some sound event done
        //RuntimeManager.PlayOneShot(soundsVolTest);
    }

    public void MuteAudio(bool isMuted)
    {
        MasterBus.setMute(isMuted);
        MusicBus.setMute(isMuted);
        SoundsBus.setMute(isMuted);
    }
}
