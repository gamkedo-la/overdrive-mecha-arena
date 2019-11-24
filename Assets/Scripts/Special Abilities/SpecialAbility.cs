using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAbility : MonoBehaviour
{
    [SerializeField] private GameObject specialAbilityVFX;
    [SerializeField] private Mecha mech;

    private float specialCooldown = 120.0f;
    private float specialUseTimeLimit = 30.0f;

    private float specialCooldownTimer;
    private float specialUseTimer;

    protected bool isSpecialInUse = false;

    private GameObject spawnedVFX;

    public Mecha _mech { get { return mech; } }

    private void Start()
    {
        Health health = GetComponent<Health>();

        mech = health._mech;

        print(gameObject.name + " is a " + mech.mechType);

        specialCooldown = mech.specialCooldown;
        specialUseTimeLimit = mech.specialUseTimeLimit;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!isSpecialInUse) // if special is not use and if cooldown is complete we can use our special
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
            if (specialUseTimer >= specialUseTimeLimit)
            {
                isSpecialInUse = false;
                Destroy(spawnedVFX);
            }
        }
    }

    protected virtual void UseSpecialAbility()
    {
        isSpecialInUse = true;

        // Execute mech's special ability
        spawnedVFX = Instantiate(specialAbilityVFX, gameObject.transform);
    }

    protected bool CanUseSpecial()
    {
        return specialCooldownTimer >= specialCooldown;
    }
}
