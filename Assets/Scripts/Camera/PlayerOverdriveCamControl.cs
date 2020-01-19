using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerOverdriveCamControl : MonoBehaviour
{
    // Will need for later implementation of drunk noise profile for vcam
    [SerializeField] private NoiseSettings drunkenProfile;

    private CinemachineVirtualCamera vcam;

    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        vcam.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    public void SetDrunkenNoise(bool shouldBeDrunk)
    {
        if (shouldBeDrunk)
        {
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = drunkenProfile;
        }
        else
        {
            // TODO: Figure out how stop drunken camera noise if the player is not above their base HP
        }
    }
}
