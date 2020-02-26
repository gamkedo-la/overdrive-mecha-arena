using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LookAtMechSelection : MonoBehaviour
{
    [SerializeField] private List<Transform> mechs;
    [SerializeField] private Light spotLight;

    private CinemachineVirtualCamera vcam;
    private int selectionIndex = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        vcam = GetComponent<CinemachineVirtualCamera>();
        vcam.m_LookAt = mechs[selectionIndex];
        spotLight.transform.LookAt(mechs[selectionIndex].transform);

        Debug.Log(mechs.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(selectionIndex <= 0)
            {
                selectionIndex = mechs.Count - 1;
            }
            else
            {
                selectionIndex--;
            }

            vcam.m_LookAt = mechs[selectionIndex];
            spotLight.transform.LookAt(mechs[selectionIndex].transform);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
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
        }
    }
}
