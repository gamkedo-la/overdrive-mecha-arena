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
    /// Now if the target if moving then we will used a multiplier of 0.75 which results in an overall 75% chance of the shooter landing a hit.
    /// In another scenario, we the same shooter at 20% HP and also moving.
    /// There are four HP penalty zones: 100% = no penalty, 80% = 12.5%, 60% = 25%, 40% = 37.5%, 20% = 50%
    /// That shooter now has an accuracy of 50% (100 accuracy - 50 penalty) minus 10% (movement penalty).
    /// Now if the target is moving, that equation would be 40% x 0.75 which equals a 30% chance of landing a hit.
    /// </summary>
    protected float accuracy;
    protected float healthPenalty;

    private float movementPenalty;
    private float dashPenalty;

    protected float hitProbability;
    protected float breakContactAtThisRange;

    public float getBreakContactRange { get { return breakContactAtThisRange; } }
    public float _movementPenalty { get { return movementPenalty; } }
    public float _dashPenalty { get { return dashPenalty; } }

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
        healthPenalty = 0.0f;

        movementPenalty = 10.0f;
        dashPenalty = 15.0f;

        hitProbability = 1.0f;
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
}
