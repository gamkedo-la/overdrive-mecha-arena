using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCombatMode : MonoBehaviour
{
    //NOTE: Light mode is on by default
    private bool heavyModeOn = false;
    [SerializeField] GameObject heavyModeVFX;
    [SerializeField] GameObject lightModeVFX;

    public bool getHeavyModeStatus { get { return heavyModeOn; } }

    void Start()
    {
        heavyModeOn = false;
        lightModeVFX.SetActive(true);
    }


    void Update()
    {
        if (Input.GetButtonDown("Transform"))
        {
            ChangeMode();
        }

        //if (heavyModeOn)
        //{
        //    Debug.Log("Heavy configuration loaded. Melee module enabled; Shooting module disabled.");
        //}
        //else
        //{
        //    Debug.Log("Light configuration loaded. Shooting module enabled; Melee module disabled.");
        //}


        //Debug.Log("Heavy Mode On? " + heavyModeOn);
    }

    private void ChangeMode()
    {
        heavyModeOn = !heavyModeOn;

        lightModeVFX.SetActive(!heavyModeOn);
        heavyModeVFX.SetActive(heavyModeOn);
    }
}
