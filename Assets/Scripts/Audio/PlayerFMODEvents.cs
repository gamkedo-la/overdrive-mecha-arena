using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFMODEvents : MonoBehaviour
{
    FMOD.Studio.EventInstance SFX_Dash;
    FMOD.Studio.PARAMETER_ID dashParameterID;
    FMOD.Studio.EventInstance SFX_MechaVoice;
    FMOD.Studio.EventInstance SFX_Random_MechaVoice;
    FMOD.Studio.EventInstance SFX_Forcefield;

    Timer mechaSpeechTimer;
    float mechaSpeechTimerDuration;

    GameObject Shield;
    // Start is called before the first frame update
    void Awake()
    {
        //Dash sound
        SFX_Dash = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/SFX_Dash");
        FMOD.Studio.EventDescription dashEventDescription;
        SFX_Dash.getDescription(out dashEventDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION dashParameterDescription;
        dashEventDescription.getParameterDescriptionByName("EndDash", out dashParameterDescription);
        dashParameterID = dashParameterDescription.id;

        //Forcefield Sound
        SFX_Forcefield = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/SFX_Forcefield");
        Shield = new GameObject();

        //Mecha Voice Sound
        SFX_MechaVoice = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/SFX_MechaVoice");
        SFX_Random_MechaVoice = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/SFX_Random_MechaVoice");
    }

    private void Start()
    {
        Shield = gameObject.transform.Find("Shield").gameObject;
        SFX_MechaVoice.start();
        SFX_MechaVoice.release();

        //Timer setup
        mechaSpeechTimer = gameObject.AddComponent<Timer>();
        mechaSpeechTimer.Duration = Random.Range(8, 15);
        mechaSpeechTimer.Run();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SFX_Dash.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        SFX_Forcefield.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        SFX_Random_MechaVoice.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

        if (Shield.activeSelf && (PlaybackState(SFX_Forcefield) != FMOD.Studio.PLAYBACK_STATE.PLAYING))
        {
            PlayForcefieldSound();
        }

        else if (!Shield.activeSelf && (PlaybackState(SFX_Forcefield) == FMOD.Studio.PLAYBACK_STATE.PLAYING))
        {
            StopForcefieldSound();
        }

        if(mechaSpeechTimer.Finished)
        {
            mechaSpeechTimer.Stop();
            SFX_Random_MechaVoice.start();
            ResetTimer();
        }
    }

    public void PlayDashSound()
    {
        SFX_Dash.setParameterByID(dashParameterID, 0);
        SFX_Dash.start();
    }

    public void StopDashSound()
    {
        SFX_Dash.setParameterByID(dashParameterID, 1);
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

    void ResetTimer()
    {
        mechaSpeechTimer.Duration = Random.Range(8, 15);
        mechaSpeechTimer.Run();
    }
}
