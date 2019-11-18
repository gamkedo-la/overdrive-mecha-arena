using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShooting : Shooting
{
    [SerializeField] private ParticleSystem shotImpact;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ScriptableObject mecha;

    [SerializeField] private Transform shotOrigin;

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
    public void FireWeapon(Health tgt, float baseSpeed, float dashSpeed)
    {
        if (CanAttack())
        {
            shotTimer = 0;

            // check my health and if I'm moving, then set my accuracy according to those parameters
            accuracy = SetAccuracyAccordingToHealthAndMovement(health.getCurrentHP, _speed, baseSpeed, dashSpeed);

            Attack(tgt, accuracy);
        }
    }

    private void Attack(Health tgt, float accuracy)
    {
        shotTimer = 0;

        var distance = Vector3.Distance(shotOrigin.position, tgt.transform.position);

        // have linecast spawn from character's chest height instead of their feet (use an empty GO and use that as the shot origin pos)
        if (!Physics.Linecast(shotOrigin.position, tgt.transform.position) && distance <= range) //Only attack if line of sight is clear and target is within range
        {
            //accuracy = ReduceAccuracyBasedOffTgtMovement();
            //Random random = GetRandomBetween(0.0f, 100.0f);
            /*if(ShotLanded())
             * {
             *    tgt.TakeDamage(damage);
             * }
             */
        }
        else
        {
            if(distance <= range)
            {
                // Move to an position free of obstructions if target is within range (hanlde this movement within ChaseState)
            }
            // Otherwise, do nothing here because ChaseState will automatically break contact and choose a new target OR it will make AI return to PatrolState
        }

        // if those conditions are not met, then check if my target is moving then find out if they are dashing or not
        // then take their movement penalty and subtract my accuracy by it
        // Now get a random float between 0  and 100 and only damage the target if that random is less than or equal to our accuracy
        tgt.TakeDamage(damage);
    }
}
