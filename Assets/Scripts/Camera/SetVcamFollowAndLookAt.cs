using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SetVcamFollowAndLookAt : MonoBehaviour
{
    [SerializeField] private GameObject VcamGO;
    [SerializeField] private Transform camLookAt;
    private CinemachineVirtualCamera vcamScript;

    private void Awake()
    {
        VcamGO = GameObject.Find("CM vcam1");
        vcamScript = VcamGO.GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        vcamScript.Follow = transform;
        vcamScript.LookAt = camLookAt;
    }
}
