using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LookAtMechSelection : MonoBehaviour
{
    [SerializeField] private List<Transform> mechs;
    [SerializeField] private Light spotLight;
    [SerializeField] private Mecha[] mechaStats;

    [SerializeField] private TextMeshProUGUI fireRate;
    [SerializeField] private TextMeshProUGUI range;
    [SerializeField] private TextMeshProUGUI speed;
    [SerializeField] private TextMeshProUGUI shields;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI special;
    [SerializeField] private TextMeshProUGUI shieldCharges;
    [SerializeField] private TextMeshProUGUI damage;

    private CinemachineVirtualCamera vcam;
    private int selectionIndex = 0;

    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        vcam.m_LookAt = mechs[selectionIndex];
        spotLight.transform.LookAt(mechs[selectionIndex].transform);

        fireRate.text = "Fire Rate: " + mechaStats[selectionIndex].fireRate;
        range.text = "Range: " + mechaStats[selectionIndex].range;
        speed.text = "Speed: " + mechaStats[selectionIndex].fowardMoveSpeed;
        shields.text = "Shields: " + mechaStats[selectionIndex].defense;
        health.text = "Health: " + mechaStats[selectionIndex].health;
        special.text = "Special: " + mechaStats[selectionIndex].specialName;
        shieldCharges.text = "Shield Charges: " + mechaStats[selectionIndex].shieldCharges;
        damage.text = "Damage: " + mechaStats[selectionIndex].damage;
    }

    public void ViewRight()
    {
        if (selectionIndex >= mechs.Count - 1)
        {
            selectionIndex = 0;
        }
        else
        {
            selectionIndex++;
        }

        vcam.m_LookAt = mechs[selectionIndex];
        spotLight.transform.LookAt(mechs[selectionIndex].transform);

        fireRate.text = "Fire Rate: " + mechaStats[selectionIndex].fireRate;
        range.text = "Range: " + mechaStats[selectionIndex].range;
        speed.text = "Speed: " + mechaStats[selectionIndex].fowardMoveSpeed;
        shields.text = "Shields: " + mechaStats[selectionIndex].defense;
        health.text = "Health: " + mechaStats[selectionIndex].health;
        special.text = "Special: " + mechaStats[selectionIndex].specialName;
        shieldCharges.text = "Shield Charges: " + mechaStats[selectionIndex].shieldCharges;
        damage.text = "Damage: " + mechaStats[selectionIndex].damage;
    }

    public void ViewLeft()
    {
        if (selectionIndex <= 0)
        {
            selectionIndex = mechs.Count - 1;
        }
        else
        {
            selectionIndex--;
        }

        vcam.m_LookAt = mechs[selectionIndex];
        spotLight.transform.LookAt(mechs[selectionIndex].transform);

        fireRate.text = "Fire Rate: " + mechaStats[selectionIndex].fireRate;
        range.text = "Range: " + mechaStats[selectionIndex].range;
        speed.text = "Speed: " + mechaStats[selectionIndex].fowardMoveSpeed;
        shields.text = "Shields: " + mechaStats[selectionIndex].defense;
        health.text = "Health: " + mechaStats[selectionIndex].health;
        special.text = "Special: " + mechaStats[selectionIndex].specialName;
        shieldCharges.text = "Shield Charges: " + mechaStats[selectionIndex].shieldCharges;
        damage.text = "Damage: " + mechaStats[selectionIndex].damage;
    }

    public void StartMatch()
    {
        PlayerPrefs.SetString("Player Mech", mechaStats[selectionIndex].mechType);
        PlayerPrefs.Save();
        SceneManager.LoadScene(2);
    }
}
