using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;

    private Image healthBar;

    private void Awake()
    {

        healthBar = GetComponent<Image>();
    }

    private void Update()
    {
        if (playerHealth.getCurrentHealthForUIPurposes > 0)
        {
            healthBar.fillAmount = playerHealth.getCurrentHealthForUIPurposes / 100.0f;
        }
    }
    public void SetPlayerHealthScript(Health health)
    {
        playerHealth = health;
    }
}
