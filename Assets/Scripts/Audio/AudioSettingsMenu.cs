using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioSettingsMenu : MonoBehaviour
{
    //TODO: Preserves audio settings for other scenes
    [EventRef]
    private string soundsVolTest;

    FMOD.Studio.Bus MasterBus;
    FMOD.Studio.Bus MusicBus;
    FMOD.Studio.Bus SoundsBus;

    float MasterBusVol = 1.0f;
    float MusicBusVol = 1.0f;
    float SoundsBusVol = 1.0f;

    private void Start()
    {
        MasterBus = RuntimeManager.GetBus("bus:/Master");
        MusicBus = RuntimeManager.GetBus("bus:/Master/Music");
        SoundsBus = RuntimeManager.GetBus("bus:/Master/Sounds");

        soundsVolTest = "event:/UI/UI_MenuSelection";
    }

    // Update is called once per frame
    void Update()
    {
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
        RuntimeManager.PlayOneShot(soundsVolTest);
    }

    public void MuteAudio(bool isMuted)
    {
        MasterBus.setMute(isMuted);
        MusicBus.setMute(isMuted);
        SoundsBus.setMute(isMuted);
    }
}
