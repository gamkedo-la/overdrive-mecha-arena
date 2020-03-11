using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePlayerSpecialIcons : MonoBehaviour
{
    [SerializeField] private GameObject specialEnabledIcon;
    [SerializeField] private GameObject specialDisabledIcon;

    private void Start()
    {
        specialDisabledIcon.SetActive(true);
        specialEnabledIcon.SetActive(false);
    }

    public void IsSpecialReady(bool isReady)
    {
        specialDisabledIcon.SetActive(!isReady);
        specialEnabledIcon.SetActive(isReady);
    }
}
