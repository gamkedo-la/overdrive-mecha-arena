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

    protected float specialCooldownTimer;
    protected float specialUseTimer;

    protected bool isSpecialInUse = false;

    private GameObject spawnedVFX;

    public Mecha _mech { get { return mech; } }

    protected virtual void Start()
    {
        Health health = GetComponent<Health>();

        mech = health._mech;

        //print(gameObject.name + " is a " + mech.mechType);

        specialCooldown = mech.specialCooldown;
        specialUseTimeLimit = mech.specialUseTimeLimit;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(CanUseSpecial() && gameObject.tag != "Player")
        {
            // note: DoubleStatsSpecial doesn't have useSpecial yet, leaving that file alone since I know it's being worked on
            gameObject.SendMessage("UseSpecial",SendMessageOptions.DontRequireReceiver);
        }

        if (!isSpecialInUse) // if special is not use and if cooldown is complete we can use our special
        {
            //Debug.Log(specialCooldownTimer);
            specialCooldownTimer += Time.deltaTime;
        }
        else // special is in use and we are now waiting for it to run out of uses
        {
            //Debug.Log("Special Ability Time Left: " + specialUseTimer);
            specialUseTimer += Time.deltaTime;

            if (specialUseTimer >= specialUseTimeLimit)
            {
                isSpecialInUse = false;
                specialUseTimer = 0.0f;
            }
        }
    }

    public bool CanUseSpecial()
    {
        return specialCooldownTimer >= specialCooldown;
    }
}
