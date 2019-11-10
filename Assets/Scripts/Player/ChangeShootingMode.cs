using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShootingMode : MonoBehaviour
{
    // TODO: Refactor so both AI and player can use this script
    // TODO: Update ChangeMode() to check if health is above 50% and only change modes if it is. Additionally, have ChangeMode set heavyModeOn to false if it's not.

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

        //Debug.Log("Heavy Mode On? " + heavyModeOn);
    }

    private void ChangeMode()
    {
        heavyModeOn = !heavyModeOn;

        lightModeVFX.SetActive(!heavyModeOn);
        heavyModeVFX.SetActive(heavyModeOn);
    }
}
