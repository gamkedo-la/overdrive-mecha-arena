using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private Mecha mech;
    [SerializeField] private GameObject shieldGO;
    [SerializeField] private float shieldToHealthConversionLimitMultiplier = 1.5f;
    private int startingHP = 200;
    private float normalShieldLevels;
    private float shields = 100;

    private PlayerOverdriveCamControl playerOverdrive;
    private SetVcamFollowAndLookAt setVcamScript;

    private Transform myAttacker;

    private float respawnTimer = 0.0f;

    private float currentHP;

    private string[] priorityValues = new string[] { "high", "medium", "low" };

    // Next three lines used to determine how valuable this target is according to how much HP it has left
    private int highPriorityThreshold = 50;
    private int mediumPriorityThreshold = 100;
    private int lowPriorityThreshold = 150;

    private string targetPriority;

    private bool isInvulnerable = false;

    private SpawnParticipantIfAble respawnManager;
    public Mecha _mech { get { return mech; } }
    public bool _isInvulnerable { set { isInvulnerable = value; } }
    public Transform _myAttacker { set { myAttacker = value; } get { return myAttacker; } }

    public float getPriorityScore
    {
        get
        {
            if (targetPriority == "high")
            {
                return 0.0f;
            }
            else if (targetPriority == "medium")
            {
                return 50.0f;
            }
            else
            {
                return 100.0f;
            }
        }
    }
    public int getBaseHP { get { return startingHP; } }
    public float getCurrentHealthAsPercentage { get { return currentHP / startingHP * 100; } }
    public float getCurrentHP { get { return currentHP; } }
    public float healthRegen { set { currentHP += value; } }
    public int _shieldLevels { get { return (int)(shields / normalShieldLevels * 100); } }

    private float timer;
    [SerializeField] private float shieldRechargeDelay = 7.5f;
    private float shieldRechargeRate = 1f;
    private bool isUsingShield = false;
    public bool _isUsingShield { set { isUsingShield = value; } }// Used by AI to turn shield on or off

    private void Start()
    {
        if (!gameObject.CompareTag("Non-playables"))
        {
            startingHP = mech.health;
            currentHP = startingHP;
            normalShieldLevels = mech.defense;
            shields = mech.defense;
        }

        respawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnParticipantIfAble>();

        if (gameObject.CompareTag("Player"))
        {
            setVcamScript = GetComponent<SetVcamFollowAndLookAt>();
            playerOverdrive = setVcamScript._vcam.GetComponent<PlayerOverdriveCamControl>();
        }

        ShouldUseShield(false);
    }

    private void Update()
    {
        if (!gameObject.CompareTag("Non-playables") && !gameObject.CompareTag("Player") && myAttacker != null)// AI debugging to retreat behavior
        {
            //Debug.Log(gameObject.name + "'s attacker is: " + myAttacker.name);
        }

        //if (myAttacker != null && !gameObject.CompareTag("Player") && !gameObject.CompareTag("Non-playables"))
        //{
        //    var distanceFromAttacker = Vector3.Distance(gameObject.transform.position, myAttacker.position);
        //    if (distanceFromAttacker > myAttacker.GetComponent<Health>()._mech.range)
        //    {
        //        myAttacker = null;
        //    }
        //}

        HandlePlayerShield();

        if (shields <= normalShieldLevels)
        {
            RechargeShieldIfAble();
        }

        TurnDrunkenessOnOrOff();
    }

    private void HandlePlayerShield()
    {
        if (gameObject.CompareTag("Player"))
        {
            if (shields > 0)
            {
                ShouldUseShield(Input.GetButton("Fire3"));
            }
            else
            {
                ShouldUseShield(false);
            }
        }
    }

    private void ShouldUseShield(bool isOn)
    {
        _isUsingShield = isOn;
        shieldGO.SetActive(isOn);
    }

    private void TurnDrunkenessOnOrOff()
    {
        if (gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player has " + currentHP + " out of " + startingHP);
            if (currentHP > startingHP)
            {
                playerOverdrive.SetDrunkenNoise(true);
            }
            else
            {
                playerOverdrive.SetDrunkenNoise(false);
            }
        }
        else if (gameObject.CompareTag("Enemy") && !gameObject.CompareTag("Non-playables"))
        {
            if (currentHP > startingHP)
            {
                // Enabled drunken behavior for AI mechs
            }
            else
            {
                // Disable drunken behavior for AI mechs
            }
        }
    }

    private void RechargeShieldIfAble()
    {
        if (!isUsingShield)
        {
            timer += Time.deltaTime;

            if (timer >= shieldRechargeDelay)
            {
                shields += shieldRechargeRate * Time.deltaTime;
            }
        }
        else
        {
            timer = 0f;
        }
    }

    public bool IsDying()
    {
        return currentHP <= 0;
    }

    private void OnEnable()
    {
        CancelInvoke("SpawnCooldown");
        respawnTimer = 0;
        currentHP = startingHP;
    }

    public void TakeDamage(int damageAmount, Transform attacker)
    {
        if (!isInvulnerable)
        {
            //update myAttacker to reflect this agent's current attacker; this will then be available to the Retreat State so this agent can run away from the attacker
            if (attacker.CompareTag("Player") || attacker.CompareTag("Enemy"))
            {
                myAttacker = attacker;
            }

            if (currentHP <= currentHP / 3)
            {
                // Set combat mode to heavy mode and prevent this mech from switching until they are no longer low on HP
            }

            if (isUsingShield && shields > 0)
            {
                shields -= damageAmount;
                if (shields <= 0)
                {
                    //DetonateElectricalCharge();
                }
            }
            else
            {
                currentHP -= damageAmount;
                Debug.Log(gameObject.name + " took " + damageAmount + " damage, now has hp: " + currentHP);


                SetMyValueAsATarget();

                if (currentHP <= 0)
                {
                    Die();
                }
            }
        }
        else
        {
            //Debug.Log(gameObject.name + " is INVULNERABLE!!!");
        }
    }
    public void StealShieldAndConvertToHP(int damage, Transform attacker, Health callerHealth)
    {
        if (!isInvulnerable && shields > 0)
        {
            shields -= damage;
            //Debug.Log(gameObject.name + " took " + damage + " shield damage, now has: " + shields);

            //update myAttacker to reflect this agent's current attacker; this will then be available to the Retreat State so this agent can run away from the attacker
            if (attacker.CompareTag("Player") || attacker.CompareTag("Enemy"))
            {
                myAttacker = attacker;
            }

            if (shields <= 0)
            {
                //DetonateElectricalCharge();
            }

            if (callerHealth.getCurrentHP <= callerHealth.getBaseHP * shieldToHealthConversionLimitMultiplier)
            {
                callerHealth.healthRegen = damage;
                //Debug.Log(callerHealth.gameObject.name + " converted " + damage + " enemy shields points for health");
            }
        }
        else
        {
            //Debug.Log(gameObject.name + " is INVULNERABLE OR HAS NO SHIELDS!!!");
        }
    }

    private void SetMyValueAsATarget()
    {
        if (currentHP >= lowPriorityThreshold)
        {
            targetPriority = priorityValues[2];
        }
        else if (currentHP >= mediumPriorityThreshold && currentHP < lowPriorityThreshold)
        {
            targetPriority = priorityValues[1];
        }
        else
        {
            // This GO has low HP and so it becomes an easier target to destroy
            targetPriority = priorityValues[0];
        }
    }

    private void Die()
    {
        InvokeRepeating("SpawnCooldown", 0.0f, 1.0f);
        gameObject.SetActive(false);
    }

    public void ResetCharacter(Transform spawnPoint)
    {
        gameObject.transform.position = spawnPoint.position;
        gameObject.SetActive(true);
    }

    private void SpawnCooldown()
    {
        respawnTimer++;

        if (respawnManager != null && respawnTimer >= respawnManager._respawnRate)
        {
            respawnManager.Respawn(gameObject);
        }
    }
}
