using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoHandling : MonoBehaviour
{
    /// <summary>
    /// Separate for handling mech ammo changes.
    /// Using integers to denote damage (1) and regen (2) shots
    /// There are better ways to do this but for our purpose and current timeline this will suffice
    /// </summary>

    private int[] ammoTypes = new int[] { 1, 2 };
    public int[] _ammoTypes { get { return ammoTypes; } }

    private int currentAmmoType = 1;
    public int _currentAmmoType { get { return currentAmmoType; } }

    private int ammoToBeLoaded;

    [SerializeField] private float ammoChangeDelay = 2.5f;
    private float delayTimer = 0.0f;

    private bool isAmmoLoaded = true;
    public bool _isAmmoLoaded { get { return isAmmoLoaded; } }

    private void Start()
    {
        delayTimer = ammoChangeDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.CompareTag("Player") && Input.GetButton("Change Ammo") && isAmmoLoaded)
        {
            if(currentAmmoType == ammoTypes[0])
            {
                ChangeAmmo(ammoTypes[1]);
            }
            else
            {
                ChangeAmmo(ammoTypes[0]);
            }
        }

        LoadAmmoIfChanged();
    }

    private void LoadAmmoIfChanged()
    {
        if (isAmmoLoaded == false)
        {
            //Debug.Log("Time until is ammo change: " + delayTimer);

            delayTimer -= Time.deltaTime;
            if (delayTimer <= 0.0f)
            {
                isAmmoLoaded = true;
                currentAmmoType = ammoToBeLoaded;
                delayTimer = ammoChangeDelay;
            }
        }
    }

    public void ChangeAmmo(int ammoType)
    {
        // Set isAmmoLoaded to false
        isAmmoLoaded = false;
        // Change to ammoType
        ammoToBeLoaded = ammoType;
        // Start ammo change delay countdown and decrease it by Time.deltaTime
        // Once countdown is complete set isAmmoLoaded to true
        // In the AI and Player shooting scripts allow shots to occur if isAmmoLoaded is true
        // Which means those scripts and any code calling their shooting methods will need an AmmoHandling reference and also need the above line's restriction
    }
}
