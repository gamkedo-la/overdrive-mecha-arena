using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleStatsSpecial : SpecialAbility
{
    private bool areStatsBuffed = false;
    public bool _areStatsBuffed { get { return areStatsBuffed; } }

    protected override void Update()
    {
        base.Update();

        if (CanUseSpecial())
        {
            if (Input.GetButtonDown("Fire3") && gameObject.tag == "Player")
            {
                UseDoubleStatsSpecial();
            }
        }
        if (!isSpecialInUse)
        {
            areStatsBuffed = isSpecialInUse;
        }
    }

    public void UseDoubleStatsSpecial()
    {
        specialCooldownTimer = 0.0f;
        isSpecialInUse = true;
        areStatsBuffed = isSpecialInUse;
    }
}
