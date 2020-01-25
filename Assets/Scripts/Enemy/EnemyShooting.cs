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
        //Calls start function of Shooting script
        base.Start();

        damage = mech.damage;
        range = mech.range;
        fireRate = mech.fireRate;
        breakContactAtThisRange = range;

        health = GetComponent<Health>();
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
    public void FireWeapon(Health tgt, float baseSpeed, float dashSpeed, bool shouldEnterOverdrive)
    {
        accuracy = 100.0f; //will be reduce by SetAccuracy and ReduceAccuracy functions

        if (!tgt.CompareTag("Non-playables"))
        {
            if (CanAttack())
            {
                shotTimer = 0;

                // check my health and if I'm moving, then set my accuracy according to those parameters
                accuracy = SetAccuracyAccordingToHealthAndMovement(health.getCurrentHealthAsPercentage, _speed, baseSpeed, dashSpeed, isTryingToDash);

                //print(gameObject.name + " accuracy after AI's movement/overdrive: " + accuracy);

                if (ShouldDamageTarget(tgt) && !shouldEnterOverdrive)
                {
                    DamageTarget(tgt);
                }
                else
                {
                    tgt.StealShieldAndConvertToHP(damage, gameObject.transform, health);
                }
            }
        }
        else
        {
            tgt.TakeDamage(damage, gameObject.transform);
        }
    }

    private bool ShouldDamageTarget(Health tgt)
    {
        if(health.getCurrentHealthAsPercentage >= tgt.getCurrentHealthAsPercentage / 2)
        {
            return true;
        }

        return false;
    }

    private void DamageTarget(Health tgt)
    {
        shotTimer = 0;

        var distance = Vector3.Distance(shotOrigin.position, tgt.transform.position);
        float tgtSpeed = 0.0f;

        bool tgtIsDashing = false;

        if (tgt.GetComponent<PlayerShooting>())
        {
            PlayerShooting p = tgt.GetComponent<PlayerShooting>();
            tgtSpeed = p._speed;
            tgtIsDashing = p.isTryingToDash;
        }
        else if (tgt.GetComponent<EnemyShooting>())
        {
            EnemyShooting e = tgt.GetComponent<EnemyShooting>();
            tgtSpeed = e._speed;
            tgtIsDashing = e.isTryingToDash;
        }
        else
        {
            print("Could not get " + tgt.name + "'s shooting component");
        }

        if (!Physics.Linecast(shotOrigin.position + Vector3.up * 18.0f, tgt.transform.position) && distance <= range) //Only attack if line of sight is clear and target is within range
        {
            accuracy = ReduceAccuracyBasedOffTgtMovement(tgtSpeed, tgt._mech.dashSpeed, tgt._mech.fowardMoveSpeed, tgtIsDashing);

            float random = UnityEngine.Random.Range(0.0f, 100.0f);

            //print(gameObject.name + " accuracy after target's movement: " + accuracy);

            if (random <= accuracy)
            {
                tgt.TakeDamage(damage, gameObject.transform);
            }
        }
        else
        {
            if (distance <= range)
            {
                // Move to an position free of obstructions if target is within range (hanlde this movement within ChaseState)
            }
            // Otherwise, do nothing here because ChaseState will automatically break contact and choose a new target OR it will make AI return to PatrolState
        }
    }
}
