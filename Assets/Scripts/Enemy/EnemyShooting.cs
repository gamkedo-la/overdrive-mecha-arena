using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : Shooting
{
    [SerializeField] private ParticleSystem shotImpact;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ScriptableObject mecha;

    private Health health;

    protected override void Start()
    {
        base.Start();
        health = GetComponent<Health>();
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
    public void FireWeapon(Health tgt)
    {
        if (CanAttack())
        {
            shotTimer = 0;

            // TODO: implement accuracy so they affect the accuracy
            Attack(tgt);
        }
    }

    //Attack() here as reference for potential improvement to FireWeapon()
    private void Attack(Health tgt)
    {
        shotTimer = 0;
        
        // check my health and if I'm moving, then set my accuracy according to those parameters
        // next, check if there's no obstruction between me and my target OR if my target is out of range
        // if either is true, then I cannot damage my target and must move to a better position (hanlde this movement with AI states)
        // if those conditions are not met, then check if my target is moving then find out if they are dashing or not
        // then take their movement penalty and subtract my accuracy by it
        // Now get a random float between 0  and 100 and only damage the target if that random is less than or equal to our accuracy
        tgt.TakeDamage(damage);
    }

}
