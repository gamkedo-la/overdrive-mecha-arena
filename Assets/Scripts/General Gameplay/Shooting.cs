using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Animator animator;

    protected int damage;
    protected float range;
    protected float fireRate;
    protected float shotTimer;

    /// <summary>
    /// Accuracy pertains to the shooter and how stationary and healthy they are.
    /// Hit probability is a multiplicative value based off factors like target movement or target visibility.
    /// For example, a shooter's accuracy is 100% if they at 100% HP and also stationary.
    /// Now if the target if moving then we will subtract their movement penalty which results in an 85% - 90% (depending on movement type) chance of the shooter landing a hit.
    /// In another scenario, we the same shooter at 20% HP and also moving.
    /// There are five HP penalty zones: 100% = no penalty, 80% = 12.5%, 60% = 25%, 40% = 37.5%, 20% = 50%
    /// That shooter now has an accuracy of 50% (100 accuracy - 50 penalty) minus 10% (movement penalty).
    /// Now if the target is moving, that equation would be 40% - 10% (target's moving) which equals a 30% chance of landing a hit.
    /// </summary>
    protected float accuracy;
    protected float healthPenalty;
    protected float drunkenPenalty;

    private int[] healthPenaltyZones = new int[] { 100, 80, 60, 40, 20 };

    private float movementPenalty;
    private float dashPenalty;

    protected float breakContactAtThisRange;

    private float speed;
    private Vector3 lastPos;
    private static GameObject ACTracersEffectPrefab;

    public bool isTryingToDash = false;

    public float getBreakContactRange { get { return breakContactAtThisRange; } }
    public float _movementPenalty { get { return movementPenalty; } }
    public float _dashPenalty { get { return dashPenalty; } }
    public float _speed { get { return speed; } } 

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if(ACTracersEffectPrefab == null)
        {
            ACTracersEffectPrefab = Resources.Load("Autocannon Effect") as GameObject;
        }
        // cache mecha scriptable objects; will be used to setup shooting stats for both modes
    }

    protected virtual void Start()
    {
        // Initial values are a pseudo-reference for the light mode effects on shooting and do no take mech types into account
        damage = 1;
        range = 300.0f;
        fireRate = 0.1f;
        accuracy = 100.0f;
        healthPenalty = 12.5f;
        drunkenPenalty = 20.0f;

        movementPenalty = 20.0f;
        dashPenalty = 40.0f;

        breakContactAtThisRange = range;
    }

    protected virtual void Update()
    {
        shotTimer += Time.deltaTime;

        if (animator != null)
        {
            animator.SetBool("isShooting", false);
        }
    }

    private void FixedUpdate()
    {
        speed = Mathf.Lerp(speed, (transform.position - lastPos).magnitude, 0.5f);
        lastPos = transform.position;

        //print(speed);
    }

    protected virtual void FireWeapon(Transform bulletPool, bool isPlayer, bool isHealthRegenShot, bool playerInOverdrive = false)
    {
        Ray ray;
        if (isPlayer)
        {
            ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        }
        else
        {
            // TODO: instantiate ray at a higher position; currently it's instantiated at the AI's feet
            ray = new Ray(transform.position, transform.forward);
        }

        //Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 2.0f);

        RaycastHit hitInfo;

        GameObject bullet = bulletPool.GetComponent<BulletObjectPool>().GetBulletFromPool();
        Component[] children = bullet.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem childParticleSystem in children)
        {
            if (childParticleSystem.name == "Muzzle Flash")
            {
                childParticleSystem.Clear();
                childParticleSystem.Play();
            }
        }

        if(animator != null)
            animator.SetBool("isShooting", true);

        // Might have a bug here with the layerMask and AI's not shooting both AI's and player properly
        int layerMask = 1 << 10;
        layerMask = ~layerMask;

        Vector3 targetPtForEffect;

        if (Physics.Raycast(ray, out hitInfo, range, layerMask, QueryTriggerInteraction.Ignore))
        {
            targetPtForEffect = hitInfo.point;
            foreach (ParticleSystem childParticleSystem in children)
            {
                if (childParticleSystem.name == "Shot Impact Particles")
                {
                    childParticleSystem.transform.position = hitInfo.point;
                    childParticleSystem.Clear();
                    childParticleSystem.Play();
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_ElectricImpact", childParticleSystem.transform.position);

                }
            }

            //Debug.Log("Shot hit: " + hitInfo.collider.name);

            var health = hitInfo.collider.GetComponent<Health>();
            if (health != null && !isHealthRegenShot)
            {
                health.TakeDamage(damage, gameObject.transform, playerInOverdrive);
            }
            else if(health != null && isHealthRegenShot)
            {
                health.StealShieldAndConvertToHP(damage, gameObject.transform, gameObject.GetComponent<Health>());
            }
        }
        else
        {
            targetPtForEffect = ray.origin + ray.direction * 100.0f; // aim effect toward gun direction
        }

        GameObject newTracerEffect = GameObject.Instantiate(ACTracersEffectPrefab);

        if (animator != null)
        {
            newTracerEffect.transform.position = animator.transform.position
                                                + animator.transform.forward * 25.0f // avoid body
                                                + animator.transform.right * 2.7f // offcenter hand
                                                + Vector3.up * 21.0f; // above feet
            newTracerEffect.transform.rotation =
                Quaternion.LookRotation(targetPtForEffect - newTracerEffect.transform.position);
        }
        else
        {
            newTracerEffect.transform.position = ray.origin;
            newTracerEffect.transform.rotation = Quaternion.LookRotation(ray.direction);
        }

    }

    protected float SetAccuracyAccordingToHealthAndMovement(float hpPercentage, float movementSpeed, float baseSpeed, float dashSpeed, bool isDashing)
    {
        float currentAccuracy = accuracy;

        //NOTE: Unable to test until further AI movement improvements are made
        if (isDashing)
        {
            currentAccuracy -= dashPenalty;
        }
        else if (movementSpeed >= 0.25f)
        {
            currentAccuracy -= movementPenalty;
        }
        else
        {

        }

        if(hpPercentage > healthPenaltyZones[0])//above 100%
        {
            currentAccuracy -= drunkenPenalty;
        }

        return currentAccuracy;
    }

    protected float ReduceAccuracyBasedOffTgtMovement(float tgtSpeed, float tgtDashSpeed, float tgtBaseSpeed, bool isDashing)
    {
        float currentAccuracy = accuracy;

        // BUG: currentAccuracy will continously be reduced while the tgt moves. This is not the intended behavior.
        // What should be happening is that currentAccuracy should only get reduced once based on which tgt movement is detected.
        // If there is no movement then we should back the accuracy penalty to the attacker

        //Debug.Log("Target speed: " + tgtSpeed + ", tgtBaseSpeed:" + tgtBaseSpeed);

        if (isDashing)
        {
            currentAccuracy -= dashPenalty;
        }
        else if (tgtSpeed >= 0.25f)
        {
            currentAccuracy -= movementPenalty;
        }
        else
        {
        } //if not moving then accuracy remains unchanged;

        return currentAccuracy;
    }
}
