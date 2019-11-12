using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int startingHP = 200;
    [SerializeField] private float defense = 100;
    public int getBaseHP { get { return startingHP; } }

    private int currentHP;
    public int getCurrentHP { get { return currentHP / 2; } }

    private string[] priorityValues = new string[] { "high", "medium", "low" };

    // Next three lines used to determine how valuable this target is according to how much HP it has left
    private int highPriorityThreshold = 50;
    private int mediumPriorityThreshold = 100;
    private int lowPriorityThreshold = 150;

    private string targetPriority;
    public float getPriorityScore
    {
        get
        {
            if(targetPriority == "high")
            {
                return 50.0f;
            }
            else if(targetPriority == "medium")
            {
                return 75.0f;
            }
            else
            {
                return 100.0f;
            }
        }
    }

    private void OnEnable()
    {
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

    private void Die()
    {
        Destroy(gameObject);
    }
}
