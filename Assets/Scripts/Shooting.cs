using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Animator animator;
    protected ChangeShootingMode shootingMode;

    protected int damage;
    protected float range;
    protected float fireRate;
    protected float shotTimer;

    private void Awake()
    {
        // Cache references for anims and shooting mode
    }

    protected virtual void Start()
    {
        damage = 1;
        range = 100.0f;
        fireRate = 1.0f;
    }

    protected virtual void Update()
    {
        
    }
}
