using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Animator animator;
    protected ChangeShootingMode shootingMode;
    protected ScriptableObject mecha;

    protected int damage;
    protected float range;
    protected float fireRate;
    protected float shotTimer;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        shootingMode = GetComponent<ChangeShootingMode>();
        // cache mecha scriptable objects; will be used to setup shooting stats for both modes
    }

    protected virtual void Start()
    {
        damage = 1;
        range = 100.0f;
        fireRate = 1.0f;
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
