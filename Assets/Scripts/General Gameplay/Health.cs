using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int startingHP = 200;
    [SerializeField] private float defense = 100;
    [SerializeField] private Mecha mech;

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
    public Transform _myAttacker { get { return myAttacker; } }

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

    // Make it so the next line returns a percentage based off our starting hp. For example, 250 (speed mech HP) is 100% and 125 is 50%. It will be used by the UI
    public float getCurrentHealthForUIPurposes { get { return currentHP / 2; } }
    public float getCurrentHP { get { return currentHP; } }

    private void Start()
    {
        startingHP = mech.health;
        defense = mech.defense;

        respawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnParticipantIfAble>();
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
            currentHP -= damageAmount;
            //Debug.Log(gameObject.name + " took " + damageAmount + " damage, now has hp: " + currentHP);

            //update myAttacker to reflect this agent's current attacker; this will then be available to the Retreat State so this agent can run away from the attacker
            if (attacker.CompareTag("Player") || attacker.CompareTag("Enemy"))
            {
                myAttacker = attacker;
            }

            if(currentHP <= currentHP/3)
            {
                // Set combat mode to heavy mode and prevent this mech from switching until they are no longer low on HP
            }

            SetMyValueAsATarget();

            if (currentHP <= 0)
            {
                Die();
            }
        }
        else
        {
            //Debug.Log(gameObject.name + " is INVULNERABLE!!!");
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
