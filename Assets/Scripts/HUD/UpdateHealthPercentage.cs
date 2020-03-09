using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateHealthPercentage : MonoBehaviour
{
    [SerializeField] private Health playerHealth;

    private TextMeshProUGUI TMProText;

    private void Awake()
    {
        TMProText = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        //TMProText.SetText(playerHealth.getCurrentHealthForUIPurposes.ToString() + "%");
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth.getCurrentHP > 0)
        {
            TMProText.SetText(Mathf.Round(playerHealth.getCurrentHealthAsPercentage).ToString() + "%");
        }
    }

    public void SetPlayerHealthScript(Health health)
    {
        playerHealth = health;
    }
}
