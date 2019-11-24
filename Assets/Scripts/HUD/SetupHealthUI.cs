using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupHealthUI : MonoBehaviour
{
    private Canvas HUD;
    private UpdateHealthBar uiHealthBar;
    private UpdateHealthPercentage uiHealthPercentage;

    private void Awake()
    {
        HUD = Canvas.FindObjectOfType<Canvas>();
    }

    private void Start()
    {
        uiHealthBar = HUD.GetComponentInChildren<UpdateHealthBar>();
        uiHealthPercentage = HUD.GetComponentInChildren<UpdateHealthPercentage>();

        uiHealthBar.SetPlayerHealthScript(gameObject.GetComponent<Health>());
        uiHealthPercentage.SetPlayerHealthScript(gameObject.GetComponent<Health>());
    }
}
