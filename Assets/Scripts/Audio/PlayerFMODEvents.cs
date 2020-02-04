using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFMODEvents : MonoBehaviour
{
    private FMOD.Studio.EventInstance SFX_GunshotLaser;

    //[FMODUnity.EventRef]
    //public string GunshotEvent;


    //GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        //SFX_GunshotLaser = FMODUnity.RuntimeManager.CreateInstance(GunshotEvent);
        //player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetButton("Fire1"))
        //{
        //    //SFX_GunshotLaser.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player));
        //    //SFX_GunshotLaser.start();
        //    //Debug.Log("ButtonPressed");
        //}
    }
}
