using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFMODEvents : MonoBehaviour
{
    FMOD.Studio.EventInstance SFX_Forcefield;
    GameObject Shield;
    // Start is called before the first frame update
    void Awake()
    {
        SFX_Forcefield = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/SFX_Forcefield");
        Shield = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        SFX_Forcefield.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        Shield = gameObject.transform.Find("Shield").gameObject;

        if (Shield.activeSelf && (PlaybackState(SFX_Forcefield) != FMOD.Studio.PLAYBACK_STATE.PLAYING))
        {
            PlayForcefieldSound();
        }

        else if (!Shield.activeSelf && (PlaybackState(SFX_Forcefield) == FMOD.Studio.PLAYBACK_STATE.PLAYING))
        {
            StopForcefieldSound();
        }

    }


    public void PlayForcefieldSound()
    {
        SFX_Forcefield.start();
    }

    public void StopForcefieldSound()
    {
        SFX_Forcefield.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    FMOD.Studio.PLAYBACK_STATE PlaybackState(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE pS;
        instance.getPlaybackState(out pS);
        return pS;
    }
}
