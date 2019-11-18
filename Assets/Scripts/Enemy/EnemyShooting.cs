using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShooting : Shooting
{
    [SerializeField] private ParticleSystem shotImpact;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Mecha mech;

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

            Attack(tgt);
        }
    }

    private void Attack(Health tgt)
    {
        shotTimer = 0;

        //var distance = Vector3.Distance(shotOrigin.position, tgt.transform.position);
        //float tgtSpeed = 0.0f;

        //if (tgt.GetComponent<PlayerShooting>())
        //{
        //    PlayerShooting p = tgt.GetComponent<PlayerShooting>();
        //    tgtSpeed = p._speed;
        //}
        //else
        //{
        //    EnemyShooting e = tgt.GetComponent<EnemyShooting>();
        //    tgtSpeed = e._speed;
        //}

        //// have linecast spawn from character's chest height instead of their feet (use an empty GO and use that as the shot origin pos)
        //if (!Physics.Linecast(shotOrigin.position, tgt.transform.position) && distance <= range) //Only attack if line of sight is clear and target is within range
        //{
        //    accuracy = ReduceAccuracyBasedOffTgtMovement(tgtSpeed, tgt._mech.dashSpeed, tgt._mech.fowardMoveSpeed);

        //    float random = UnityEngine.Random.Range(0.0f, 100.0f);

        //    if (random <= accuracy)
        //    {
        //        tgt.TakeDamage(damage);
        //    }
        //}
        //else
        //{
        //    if (distance <= range)
        //    {
        //        // Move to an position free of obstructions if target is within range (hanlde this movement within ChaseState)
        //    }
        //    // Otherwise, do nothing here because ChaseState will automatically break contact and choose a new target OR it will make AI return to PatrolState
        //}

        tgt.TakeDamage(damage);
    }
}
