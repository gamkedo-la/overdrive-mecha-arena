using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int startingHP = 200;
    [SerializeField] private float defense = 100;

    private int currentHP;
    public int getCurrentHP { get { return currentHP / 2; } }

    private void OnEnable()
    {
        currentHP = startingHP;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " is not destroyed. It's simply disabled for pitch demo purposes.");
        gameObject.SetActive(false);
    }
}
