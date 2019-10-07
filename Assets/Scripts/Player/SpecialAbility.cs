using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAbility : MonoBehaviour
{
    [SerializeField] private GameObject specialAbilityVFX;

    [SerializeField] private float specialCooldown = 120.0f;
    [SerializeField] private float specialUseTimeLimit = 30.0f;
    [SerializeField] private float strengthOfSpecialAbility = 20.0f;

    private float specialCooldownTimer;
    private float specialUseTimer;

    private bool isSpecialInUse = false;

    private GameObject spawnedVFX;

    // Update is called once per frame
    void Update()
    {
        if(!isSpecialInUse)
        {
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
        else
        {
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

        spawnedVFX = Instantiate(specialAbilityVFX);
    }
}
