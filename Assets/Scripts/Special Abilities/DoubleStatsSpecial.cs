using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleStatsSpecial : SpecialAbility
{
    private bool areStatsBuffed = false;
    public bool _areStatsBuffed { get { return areStatsBuffed; } }

    protected override void Start()
    {
        base.Start();

        if (gameObject.CompareTag("Player"))
        {
            SetupCanvasForPlayer();
        }
    }

    protected override void Update()
    {
        base.Update();

        if (CanUseSpecial() && gameObject.tag == "Player")
        {
            icons.IsSpecialReady(true);
            if (Input.GetButtonDown("Fire3"))
            {
                UseDoubleStatsSpecial();
            }
        }
        else if(gameObject.CompareTag("Player"))
        {
            icons.IsSpecialReady(false);
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

    public void UseSpecial()
    {
        Debug.Log("AI using UseDoubleStatsSpecial");
        UseDoubleStatsSpecial();
    }
}
