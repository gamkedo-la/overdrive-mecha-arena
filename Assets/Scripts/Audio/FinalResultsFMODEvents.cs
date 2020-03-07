using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class FinalResultsFMODEvents : MonoBehaviour
{
    private void Awake()
    {
        FMOD.Studio.Bus ambience = RuntimeManager.GetBus("bus:/Master/Sounds/Ambience");
        FMOD.Studio.Bus sfx = RuntimeManager.GetBus("bus:/Master/Sounds/SFX");
        ambience.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        sfx.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
