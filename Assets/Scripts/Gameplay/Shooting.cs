using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Animator animator;
    protected ChangeShootingMode shootingMode;

    // NOTE: Light mode features lower damage, faster fire rate, and increased range
    // NOTE: Heavy mode features higher damage, slower fire rate, and decreased range
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

    private int[] healthPenaltyZones = new int[] { 100, 80, 60, 40, 20 };

    private float movementPenalty;
    private float dashPenalty;

    protected float breakContactAtThisRange;

    private float speed;
    private Vector3 lastPos;

    public float getBreakContactRange { get { return breakContactAtThisRange; } }
    public float _movementPenalty { get { return movementPenalty; } }
    public float _dashPenalty { get { return dashPenalty; } }
    public float _speed { get { return speed; } }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        shootingMode = GetComponent<ChangeShootingMode>();
        // cache mecha scriptable objects; will be used to setup shooting stats for both modes
    }

    protected virtual void Start()
    {
        // Initial values are a pseuo-reference for the light mode effects on shooting and do no take mech types into account
        damage = 1;
        range = 300.0f;
        fireRate = 0.1f;
        accuracy = 100.0f;
        healthPenalty = 12.5f;

        movementPenalty = 10.0f;
        dashPenalty = 15.0f;

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

    protected virtual void FireWeapon(ParticleSystem shotImpact, ParticleSystem muzzleFlash, bool isPlayer)
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

        muzzleFlash.Play();

        if(animator != null)
            animator.SetBool("isShooting", true);

        // Might have a bug here with the layerMask and AI's not shooting both AI's and player properly
        int layerMask = 1 << 10;
        layerMask = ~layerMask;

        if (Physics.Raycast(ray, out hitInfo, range, layerMask, QueryTriggerInteraction.Ignore))
        {
            Instantiate(shotImpact, hitInfo.point, Quaternion.identity);

            //Debug.Log("Shot hit: " + hitInfo.collider.name);

            var health = hitInfo.collider.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }

    protected float SetAccuracyAccordingToHealthAndMovement(float hpPercentage, float movementSpeed, float baseSpeed, float dashSpeed)
    {
        float currentAccuracy = accuracy;
        
        //NOTE: Potential bug might be caused here; will find out once implemented
        if(movementSpeed >= baseSpeed) { currentAccuracy -= movementPenalty; }
        else if(movementSpeed >= dashSpeed) { currentAccuracy -= dashPenalty; }
        else { } //if not moving then accuracy remains unchanged;

        if(hpPercentage > healthPenaltyZones[0])//above 100%
        {
            //Decrease accuracy because we are in overdrive
        }
        else if(hpPercentage <= healthPenaltyZones[1] && hpPercentage > healthPenaltyZones[2])//Between 80-61%
        {
            currentAccuracy -= healthPenalty;
        }
        else if(hpPercentage <= healthPenaltyZones[2] && hpPercentage > healthPenaltyZones[3])//Between 60-41%
        {
            currentAccuracy -= (healthPenalty * 2);
        }
        else if (hpPercentage <= healthPenaltyZones[3] && hpPercentage > healthPenaltyZones[4])//Between 40-21%
        {
            currentAccuracy -= (healthPenalty * 3);
        }
        else
        {
            if(hpPercentage > 0 && hpPercentage <= healthPenaltyZones[4])//Between 20-1%
            {
                currentAccuracy -= (healthPenalty * 4);
            }
        }

        return currentAccuracy;
    }

    protected float ReduceAccuracyBasedOffTgtMovement(float tgtSpeed, float tgtDashSpeed, float tgtBaseSpeed)
    {
        float currentAccuracy = accuracy;

        if (tgtSpeed >= tgtBaseSpeed) { currentAccuracy -= movementPenalty; }
        else if (tgtSpeed >= tgtDashSpeed) { currentAccuracy -= dashPenalty; }
        else { } //if not moving then accuracy remains unchanged;

        return currentAccuracy;
    }
}
