using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFMODEvents : MonoBehaviour
{
    FMOD.Studio.EventInstance SFX_Dash;
    FMOD.Studio.PARAMETER_ID dashParameterID;
    // Start is called before the first frame update
    void Awake()
    {
        SFX_Dash = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/SFX_Dash");
        FMOD.Studio.EventDescription dashEventDescription;
        SFX_Dash.getDescription(out dashEventDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION dashParameterDescription;
        dashEventDescription.getParameterDescriptionByName("EndDash", out dashParameterDescription);
        dashParameterID = dashParameterDescription.id;
    }

    // Update is called once per frame
    void Update()
    {
        SFX_Dash.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

    }

    public void PlayDashSound()
    {
        SFX_Dash.setParameterByID(dashParameterID, 0);
        SFX_Dash.start();
        Debug.Log("start dash sound");

    }

    public void StopDashSound()
    {
        SFX_Dash.setParameterByID(dashParameterID, 1);
        Debug.Log("stop dash sound");
    }

    FMOD.Studio.PLAYBACK_STATE PlaybackState(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE pS;
        instance.getPlaybackState(out pS);
        return pS;
    }
}
