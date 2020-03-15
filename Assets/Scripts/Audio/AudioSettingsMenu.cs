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

    bool mute = false;

    private void Start()
    {
        //Debug.Log("MasterVol Pref " + PlayerPrefs.GetFloat("MasterVol"));
        //Debug.Log("MusicVol Pref " + PlayerPrefs.GetFloat("MusicVol"));
        //Debug.Log("SoundsVol Pref " + PlayerPrefs.GetFloat("SoundsVol"));

        MasterBus = RuntimeManager.GetBus("bus:/Master");
        MusicBus = RuntimeManager.GetBus("bus:/Master/Music");
        SoundsBus = RuntimeManager.GetBus("bus:/Master/Sounds");

        MasterBusVol = PlayerPrefs.GetFloat("MasterVol");
        MusicBusVol = PlayerPrefs.GetFloat("MusicVol");
        SoundsBusVol = PlayerPrefs.GetFloat("SoundsVol");
        mute = PlayerPrefs.GetString("IsMuted") == "True" ? true : false;

        if(mute)
        {
            MasterBus.setMute(mute);
            MusicBus.setMute(mute);
            SoundsBus.setMute(mute);
        }

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
        PlayerPrefs.SetFloat("MasterVol", MasterBusVol);
        PlayerPrefs.Save();
    }
    public void MusicVolumeLevel(float newMusicrVol)
    {
        MusicBusVol = newMusicrVol;
        PlayerPrefs.SetFloat("MusicVol", MusicBusVol);
        PlayerPrefs.Save();
    }
    public void SoundsVolumeLevel(float newSoundsVol)
    {
        SoundsBusVol = newSoundsVol;

        PlayerPrefs.SetFloat("SoundsVol", SoundsBusVol);
        PlayerPrefs.Save();

        // next code block used for testing sounds volume slider changes when we get some sound event done
        //RuntimeManager.PlayOneShot(soundsVolTest);
    }

    public void MuteAudio(bool isMuted)
    {
        mute = isMuted;

        MasterBus.setMute(mute);
        MusicBus.setMute(mute);
        SoundsBus.setMute(mute);

        PlayerPrefs.Save();
    }
}
