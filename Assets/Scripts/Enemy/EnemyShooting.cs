using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : Shooting
{
    [SerializeField] private ParticleSystem shotImpact;
    [SerializeField] private ParticleSystem muzzleFlash;


    protected override void Start()
    {
        base.Start();
        // Set this AI's shooting stats according to it's mecha type
    }


    protected override void Update()
    {
        // Update shotTimer in Shooting.cs
        base.Update();
    }

    private bool CanAttack()
    {
        return shotTimer >= fireRate;
    }

    // Called by AI character and it states
    public void FireWeapon()
    {
        if (CanAttack())
        {
            shotTimer = 0;

            // TODO: implement accuracy and random hit probabilities so they affect the accuracy of raycast in Shooting.cs
            // Alternatively, could create a custom function that will account for those without needing raycasts
            base.FireWeapon(shotImpact, muzzleFlash, false);
        }
    }

    /* Attack() here as reference for potential improvement to FireWeapon()
    private void Attack()
    {
        shotTimer = 0;
        targetHP.TakeDamage(damageAmount);
    }
    */
}
