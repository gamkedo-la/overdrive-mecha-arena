using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : Shooting
{
    // Must find out how the mecha selection will work in code
    // Could create a parent class Mecha and have subclasses of SpeedMecha, DamageMecha, and AllPurposeMecha
    // Scriptable objects could be a great solution for this

    [SerializeField] private ParticleSystem shotImpact;
    [SerializeField] private ParticleSystem muzzleFlash;

    protected override void Start()
    {
        base.Start();
        // Setup shot particles for player
    }

    protected override void Update()
    {
        base.Update();
        if(shotTimer >= fireRate)
        {
            if (Input.GetButton("Fire1"))
            {
                shotTimer = 0f;
                base.FireWeapon(shotImpact, muzzleFlash, true);
            }
        }
    }
}
