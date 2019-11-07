using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAndHeavyShooting : MonoBehaviour
{
    private Animator animator;
    private ChangeShootingMode combatMode;

    //TODO: Increase fireRate if light mode is on
    //TODO: Increase damage if heavy mode is on
    //TODO: Decrease shot range if heavy mode is on
    //TODO: Add shot landing VFX to container and delete each one after 2 seconds
    [SerializeField] [Range(0.25f, 5.0f)] private float fireRate = 1.0f;
    [SerializeField] [Range(1, 100)] private int damage = 1;
    [SerializeField] [Range(100.0f, 300.0f)] private float shotRange = 300.0f;
    private float timer;

    [SerializeField] private ParticleSystem shotImpact;
    [SerializeField] private ParticleSystem muzzleFlash;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        combatMode = GetComponent<ChangeShootingMode>();
    }

    void Update()
    {
        if (!combatMode.getHeavyModeStatus)
        {
            timer += Time.deltaTime;
            if (timer >= fireRate)
            {
                if (Input.GetButton("Fire1"))
                {
                    timer = 0f;
                    FireWeapon();
                }
                else
                {
                    animator.SetBool("isShooting", false);
                }
            }
        }
    }

    private void FireWeapon()
    {
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);

        //Debug.DrawRay(ray.origin, ray.direction * shotRange, Color.red, 2.0f);

        RaycastHit hitInfo;

        muzzleFlash.Play();
        animator.SetBool("isShooting", true);

        int layerMask = 1 << 10;
        layerMask = ~layerMask;

        if (Physics.Raycast(ray, out hitInfo, shotRange, layerMask, QueryTriggerInteraction.Ignore))
        {
            Instantiate(shotImpact, hitInfo.point, Quaternion.identity);
            Debug.Log("Shot hit: " + hitInfo.collider.name);
            var health = hitInfo.collider.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

        }
    }
}
