﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : Shooting
{
    // Must find out how the mecha selection will work in code
    // Could create a parent class Mecha and have subclasses of SpeedMecha, DamageMecha, and AllPurposeMecha
    // Scriptable objects could be a great solution for this

    [SerializeField] private Mecha mecha;
    private Health health;
    private ScoreHandler scoreHandler;
    private Transform bulletPool;
    private AmmoHandling ammo;

    private DoubleStatsSpecial doubleStats;
    public float _playerShootingRange { get { return range; } }

    protected override void Start()
    {
        base.Start();

        damage = mecha.damage;
        range = mecha.range;
        fireRate = mecha.fireRate;

        bulletPool = transform.Find("PlayerBulletPool");
        health = GetComponent<Health>();
        ammo = GetComponent<AmmoHandling>();

        scoreHandler = health._scoreHandler;
        if (GetComponent<DoubleStatsSpecial>() != null)
        {
            doubleStats = GetComponent<DoubleStatsSpecial>();
        }
    }

    protected override void Update()
    {
        base.Update();

        if (doubleStats != null && doubleStats._areStatsBuffed)
        {
            damage = mecha.damage * 2;
            range = mecha.range * 2;
            fireRate = mecha.fireRate / 2;
        }
        else
        {
            damage = mecha.damage;
            range = mecha.range;
            fireRate = mecha.fireRate;
        }

        if (shotTimer >= fireRate)
        {
            if (Input.GetButton("Fire1") || Input.GetAxis("Fire4") == 1)
            {
                shotTimer = 0f;

                if (ammo._currentAmmoType == ammo._ammoTypes[0] && Time.deltaTime != 0)
                {
                    base.FireWeapon(bulletPool, true, false, health.isInOverdrive());
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_GunshotLaser", transform.position);
                }
            }
            else if (Input.GetButton("Fire2") || Input.GetAxis("Fire5") == 1)
            {
                shotTimer = 0f;

                if (ammo._currentAmmoType == ammo._ammoTypes[1])
                {
                    base.FireWeapon(bulletPool, true, true);
                }
            }
        }
    }
}
