using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class PlayerFMODEvents : MonoBehaviour
{
    EventInstance SFX_Dash;
    PARAMETER_ID dashParameterID;
    EventInstance SFX_MechaVoice;
    EventInstance SFX_Random_MechaVoice;
    EventInstance SFX_Forcefield;
    EventInstance SFX_MechaMoves;
    PARAMETER_ID mechaMovementParameterID;

    Timer mechaSpeechTimer;
    float mechaSpeechTimerDuration;

    GameObject Shield;
    // Start is called before the first frame update
    void Awake()
    {
        //Dash sound
        SFX_Dash = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/SFX_Dash");
        EventDescription dashEventDescription;
        SFX_Dash.getDescription(out dashEventDescription);
        PARAMETER_DESCRIPTION dashParameterDescription;
        dashEventDescription.getParameterDescriptionByName("EndDash", out dashParameterDescription);
        dashParameterID = dashParameterDescription.id;

        // Mecha movement sounds
        SFX_MechaMoves = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/SFX_MechaMoves");
        EventDescription mechaMovementEventDescription;
        SFX_MechaMoves.getDescription(out mechaMovementEventDescription);
        PARAMETER_DESCRIPTION mechaMovementParameterDescription;
        mechaMovementEventDescription.getParameterDescriptionByName("MechaStop", out mechaMovementParameterDescription);
        mechaMovementParameterID = mechaMovementParameterDescription.id;

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
        SFX_MechaMoves.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

        if (Shield.activeSelf && (PlaybackState(SFX_Forcefield) != PLAYBACK_STATE.PLAYING))
        {
            PlayForcefieldSound();
        }

        else if (!Shield.activeSelf && (PlaybackState(SFX_Forcefield) == PLAYBACK_STATE.PLAYING))
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
        if (PlaybackState(SFX_MechaMoves) == PLAYBACK_STATE.PLAYING)
        {
            PauseUnpause(SFX_MechaMoves, true);
        }
        SFX_Dash.start();

    }

    public void StopDashSound()
    {
        SFX_Dash.setParameterByID(dashParameterID, 1);
        if (PlaybackState(SFX_MechaMoves) == PLAYBACK_STATE.PLAYING)
        {
            PauseUnpause(SFX_MechaMoves, false);
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

    public void PlayMechaMovementSound()
    {
        // Debug.Log("Play mecha sound");
        if (PlaybackState(SFX_MechaMoves) != PLAYBACK_STATE.PLAYING)
        {
            SFX_MechaMoves.setParameterByID(mechaMovementParameterID, 0);
            SFX_MechaMoves.start();
        }
    }

    public void StopMechaMovementSound()
    {
        SFX_MechaMoves.setParameterByID(mechaMovementParameterID, 1);
    }

    public void PauseUnpause (EventInstance eventToPause, bool pause)
    {
        if (pause)
        {
            eventToPause.setPaused(true);
        }
        else
        {
            eventToPause.setPaused(false);
        }
    }

    PLAYBACK_STATE PlaybackState(EventInstance instance)
    {
        PLAYBACK_STATE pS;
        instance.getPlaybackState(out pS);
        return pS;
    }

    void ResetTimer()
    {
        mechaSpeechTimer.Duration = Random.Range(8, 15);
        mechaSpeechTimer.Run();
    }
}
