﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAbility : MonoBehaviour
{
    [SerializeField] private GameObject specialAbilityVFX;

    [SerializeField] private float specialCooldown = 120.0f;
    [SerializeField] private float specialUseTimeLimit = 30.0f;
    [SerializeField] private int strengthOfSpecialAbility = 20; // Refers to how many much damage it will cause

    private float specialCooldownTimer;
    private float specialUseTimer;

    private bool isSpecialInUse = false;

    private GameObject spawnedVFX;

    // Update is called once per frame
    void Update()
    {
        if(!isSpecialInUse) // if special is not use and if cooldown is complete we can use our special
        {
            //Debug.Log(specialCooldownTimer);
            specialCooldownTimer += Time.deltaTime;
            if (specialCooldownTimer >= specialCooldown)
            {
                if (Input.GetButtonDown("Fire3"))
                {
                    specialCooldownTimer = 0.0f;
                    UseSpecialAbility();
                }
            }
        }
        else // special is in use and we are now waiting for it to run out of uses
        {
            //Debug.Log(specialUseTimer);
            specialUseTimer += Time.deltaTime;
            if(specialUseTimer >= specialUseTimeLimit)
            {
                isSpecialInUse = false;
                Destroy(spawnedVFX);
            }
        }
    }

    private void UseSpecialAbility()
    {
        isSpecialInUse = true;

        spawnedVFX = Instantiate(specialAbilityVFX, gameObject.transform);
    }
}
