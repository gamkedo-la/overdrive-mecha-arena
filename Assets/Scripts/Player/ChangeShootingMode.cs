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
    private Health health;

    public bool getHeavyModeStatus { get { return heavyModeOn; } }

    void Start()
    {
        health = GetComponent<Health>();
        heavyModeOn = false;
        lightModeVFX.SetActive(true);
    }


    void Update()
    {
        if (gameObject.tag == "Player")
        {
            if (Input.GetButtonDown("Transform"))
            {
                ChangeMode();
            }
        }

        //Debug.Log("Heavy Mode On? " + heavyModeOn);
    }

    public void ChangeMode()
    {
        if (health.getCurrentHP >= health.getBaseHP / 2)
        {
            heavyModeOn = !heavyModeOn;

            lightModeVFX.SetActive(!heavyModeOn);
            heavyModeVFX.SetActive(heavyModeOn);
        }
        else
        {
            heavyModeOn = false;
            lightModeVFX.SetActive(true);
            heavyModeVFX.SetActive(false);
        }
    }
}
