using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupShieldUI : MonoBehaviour
{
    private Canvas HUD;
    private UpdateShieldLevels shieldLevelsUI;

    void Start()
    {
        HUD = Canvas.FindObjectOfType<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        shieldLevelsUI = HUD.GetComponentInChildren<UpdateShieldLevels>();
        shieldLevelsUI.SetPlayerHealthScript(gameObject.GetComponent<Health>());
    }
}
