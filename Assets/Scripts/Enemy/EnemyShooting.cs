using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShooting : Shooting
{
    [SerializeField] private LayerMask ignoreForViewObstructionCheck;
    [SerializeField] private ParticleSystem shotImpact;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Mecha mech;

    [SerializeField] private Transform shotOrigin;

    private Health health;
    private ScoreHandler scoreHandler;
    private AmmoHandling ammo;
    public AmmoHandling _ammo { get { return ammo; } }

    private bool hasLostTgt = false;
    public bool _hasLostTgt { get { return hasLostTgt; } set { hasLostTgt = value; } }

    protected override void Start()
    {
        //Calls start function of Shooting script
        base.Start();

        damage = mech.damage;
        range = mech.range;
        fireRate = mech.fireRate;
        breakContactAtThisRange = range;

        health = GetComponent<Health>();
        ammo = GetComponent<AmmoHandling>();
        scoreHandler = health._scoreHandler;
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

                AttackTarget(tgt, shouldEnterOverdrive);
            }
        }
        else
        {
            tgt.TakeDamage(damage, gameObject.transform);
        }
    }

    private bool ShouldDamageTarget(Health tgt)
    {
        if (health.getCurrentHealthAsPercentage >= tgt.getCurrentHealthAsPercentage / 2)
        {
            return true;
        }

        return false;
    }

    private void AttackTarget(Health tgt, bool shouldEnterOverdrive)
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

        Debug.DrawLine(shotOrigin.position + Vector3.up * 18.0f, tgt.transform.position, Color.cyan);
        bool isLineOfSightBlocked = Physics.Linecast(shotOrigin.position + Vector3.up * 18.0f, tgt.transform.position + Vector3.up * 18.0f, ~ignoreForViewObstructionCheck, QueryTriggerInteraction.Ignore);

        if (!isLineOfSightBlocked && distance <= range) //Only attack if line of sight is clear and target is within range
        {
            accuracy = ReduceAccuracyBasedOffTgtMovement(tgtSpeed, tgt._mech.dashSpeed, tgt._mech.fowardMoveSpeed, tgtIsDashing);

            float random = UnityEngine.Random.Range(0.0f, 100.0f);

            //print(gameObject.name + " accuracy after target's movement: " + accuracy);

            if (random <= accuracy)
            {
                if (ShouldDamageTarget(tgt) && !shouldEnterOverdrive)
                {
                    if (ammo._currentAmmoType == ammo._ammoTypes[0])
                    {
                        tgt.TakeDamage(damage, gameObject.transform);
                    }
                    else if(ammo._isAmmoLoaded)
                    {
                        ammo.ChangeAmmo(ammo._ammoTypes[0]);
                    }
                }
                else
                {
                    if (ammo._currentAmmoType == ammo._ammoTypes[1])
                    {
                        tgt.StealShieldAndConvertToHP(damage, gameObject.transform, health);
                    }
                    else if(ammo._isAmmoLoaded)
                    {
                        ammo.ChangeAmmo(ammo._ammoTypes[1]);
                    }
                }
            }
        }
        else
        {
            hasLostTgt = true;
        }
    }
}
