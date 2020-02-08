using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensitivity = 1.0f;
    public float viewTiltMin = -20.0f;
    public float viewTiltMax = 30.0f;

    private CinemachineComposer composer;

    void Start()
    {
        composer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineComposer>();
    }

    void Update()
    {
        if (Time.timeScale > 0f)
        {
            float vertical;
            if (Input.GetJoystickNames() == null)
            {
                vertical = Input.GetAxis("Mouse Y") * sensitivity;
            }
            else
            {
                vertical = Input.GetAxis("JoyRotY");
                //Debug.Log(vertical);
            }

            composer.m_TrackedObjectOffset.y += vertical;
            composer.m_TrackedObjectOffset.y = Mathf.Clamp(composer.m_TrackedObjectOffset.y, viewTiltMin, viewTiltMax);
        }
    }
}
