using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaCoil : MonoBehaviour
{
	FMOD.Studio.EventInstance SFX_TeslaCoil;
    // Start is called before the first frame update
    void Start()
    {
		SFX_TeslaCoil = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/SFX_TeslaCoil");
		SFX_TeslaCoil.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
		SFX_TeslaCoil.start();
    }

    void Awake()
	{

	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
