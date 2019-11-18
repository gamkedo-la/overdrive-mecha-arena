using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int startingHP = 200;
    [SerializeField] private float defense = 100;

    private float respawnTimer = 0.0f;

    private float currentHP;

    private string[] priorityValues = new string[] { "high", "medium", "low" };

    // Next three lines used to determine how valuable this target is according to how much HP it has left
    private int highPriorityThreshold = 50;
    private int mediumPriorityThreshold = 100;
    private int lowPriorityThreshold = 150;

    private string targetPriority;

    private SpawnParticipantIfAble respawnManager;

    public float getPriorityScore
    {
        get
        {
            if(targetPriority == "high")
            {
                return 0.0f;
            }
            else if(targetPriority == "medium")
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

    // Make it so this returns a percentage based off our starting hp. For example, 250 (speed mech HP) is 100% and 125 is 50%.
    public float getCurrentHP { get { return currentHP / 2; } }

    private void Start()
    {
        // Set defense, starting HP, priority threshold according to mech type
        respawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnParticipantIfAble>();
    }

    private void OnEnable()
    {
        CancelInvoke("SpawnCooldown");
        respawnTimer = 0;
        currentHP = startingHP;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        //Debug.Log(gameObject.name + " took " + damageAmount + " damage, now has hp: " + currentHP);

        SetMyValueAsATarget();

        if (currentHP <= 0)
        {
            Die();
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

    private void Retreat()
    {

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

        if(respawnTimer >= respawnManager._respawnRate)
        {
            respawnManager.Respawn(gameObject);
        }
    }
}
