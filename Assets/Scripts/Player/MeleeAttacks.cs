using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttacks : MonoBehaviour
{
    private ChangeCombatMode combatMode;
    private Animator animator;

    [SerializeField] [Range(5.0f, 20.0f)] private int lightMeleeDamage = 5;
    [SerializeField] [Range(5.0f, 20.0f)] private int heavyMeleeDamage = 20;
    [SerializeField] private float lightMeleeRange = 60.0f;
    [SerializeField] private float heavyMeleeRange = 90.0f;

    private void Awake()
    {
        combatMode = GetComponent<ChangeCombatMode>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (combatMode.getHeavyModeStatus)
        {
            //TODO: figure out a way to only execute the if statement when the light melee animation is done
            if (Input.GetButtonDown("Fire2"))
            {
                DoLightMelee();
                //Debug.Log("Melee!");
            }
            else
            {
                animator.SetBool("isMeleeing", false);
            }
        }
    }

    private void DoLightMelee()
    {

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        Debug.DrawRay(ray.origin, ray.direction * lightMeleeRange, Color.red, 2.0f);
        RaycastHit hitInfo;

        animator.SetBool("isMeleeing", true);

        int layerMask = 1 << 10;
        layerMask = ~layerMask;

        if (Physics.Raycast(ray, out hitInfo, lightMeleeRange, layerMask, QueryTriggerInteraction.Ignore))
        {
            var health = hitInfo.collider.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(lightMeleeDamage);
            }
        }
    }
}
