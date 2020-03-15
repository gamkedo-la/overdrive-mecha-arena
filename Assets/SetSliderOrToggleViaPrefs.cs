using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSliderOrToggleViaPrefs : MonoBehaviour
{
    [SerializeField] private Toggle muteToggle;
    [SerializeField] private Slider masterVolSlider;
    [SerializeField] private Slider musicVolSlider;
    [SerializeField] private Slider soundVolSlider;

    void Start()
    {
        muteToggle.isOn = PlayerPrefs.GetString("IsMuted") == "True" ? true : false;
        masterVolSlider.value = PlayerPrefs.GetFloat("MasterVol");
        musicVolSlider.value = PlayerPrefs.GetFloat("MusicVol");
        soundVolSlider.value = PlayerPrefs.GetFloat("SoundsVol");
    }
}
