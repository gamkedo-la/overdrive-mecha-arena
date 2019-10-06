using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLightAndHeavyShooting : MonoBehaviour
{
    [SerializeField] [Range(0.25f, 5.0f)] private float shotRefreshRate = 1;
    [SerializeField] [Range(1, 100)] private int damageAmount = 1;
    [SerializeField] [Range(100.0f, 20.0f)] private float shotRange = 100.0f;


    private EnemyAggro aggroDetection;
    private Health targetHP;

    private float shotTimer;

    private void Awake()
    {
        aggroDetection = GetComponent<EnemyAggro>();
        aggroDetection.Aggroed += AggroDetection_Aggroed;
    }

    private void AggroDetection_Aggroed(Transform target)
    {
        Health health = target.GetComponent<Health>();
        if (health != null)
        {
            targetHP = health;
        }
    }

    private void Update()
    {
        if (targetHP != null)
        {
            shotTimer += Time.deltaTime;

            if (CanAttack())
            {
                Attack();
            }
        }
    }

    private bool CanAttack()
    {
        return shotTimer >= shotRefreshRate;
    }

    private void Attack()
    {
        shotTimer = 0;
        targetHP.TakeDamage(damageAmount);
    }
}
