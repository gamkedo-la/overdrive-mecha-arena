using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSelectedMechModel : MonoBehaviour
{
    [SerializeField] private GameObject speed;
    [SerializeField] private GameObject damage;
    [SerializeField] private GameObject allPurpose;

    void Awake()
    {
        speed.SetActive(false);
        damage.SetActive(false);
        allPurpose.SetActive(false);

        switch (PlayerPrefs.GetString("Player Mech"))
        {
            case "Speed":
                speed.SetActive(true);
                break;
            case "Damage":
                damage.SetActive(true);
                break;
            case "All-purpose":
                allPurpose.SetActive(true);
                break;
        }
    }
}
