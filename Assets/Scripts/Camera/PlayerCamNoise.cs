using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamNoise : MonoBehaviour
{
    // Will need for later implementation of drunk noise profile for vcam
    [SerializeField] private NoiseSettings drunkenProfile;
    [SerializeField] private NoiseSettings soberProfile;
    [SerializeField] private NoiseSettings dashProfile;

    private CinemachineVirtualCamera vcam;

    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        vcam.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    public void SetCamNoise(bool shouldBeDrunk)
    {
        if (shouldBeDrunk)
        {
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = drunkenProfile;
        }
        else if (Input.GetButton("Dash"))
        {
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = dashProfile;
        }
        else
        {
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = soberProfile;
        }
    }
}
