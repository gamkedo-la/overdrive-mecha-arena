using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : Shooting
{
    // Must find out how the mecha selection will work in code
    // Could create a parent class Mecha and have subclasses of SpeedMecha, DamageMecha, and AllPurposeMecha
    // Scriptable objects could be a great solution for this

    [SerializeField] private Mecha mecha;
    private Transform bulletPool;

    protected override void Start()
    {
        base.Start();

        damage = mecha.damage;
        range = mecha.range;
        fireRate = mecha.fireRate;

        bulletPool = transform.Find("PlayerBulletPool");
        // Set shooting according to which mecha the player is
    }

    protected override void Update()
    {
        base.Update();
        if(shotTimer >= fireRate)
        {
            if (Input.GetButton("Fire1"))
            {
                shotTimer = 0f;
                base.FireWeapon(bulletPool, true);
            }
        }
    }
}
