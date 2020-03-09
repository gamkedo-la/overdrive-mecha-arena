using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateShieldLevels : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    private TextMeshProUGUI TMProText;

    void Start()
    {
        TMProText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        TMProText.SetText(Mathf.Round(playerHealth._shieldLevels).ToString() + "%");
    }

    public void SetPlayerHealthScript(Health health)
    {
        playerHealth = health;
    }
}
