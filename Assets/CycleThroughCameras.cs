using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CycleThroughCameras : MonoBehaviour
{
    [SerializeField] private List<CinemachineVirtualCamera> vcams;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 8.0f)
        {
            ChooseNewCamAndRollDiceOnEnabling(UnityEngine.Random.Range(0, vcams.Count));
            timer = 0.0f;
        }
    }

    private void ChooseNewCamAndRollDiceOnEnabling(int v)
    {
        for(int i = 0; i < vcams.Count - 1; i++)
        {
            vcams[i].gameObject.SetActive(false);
        }

        if (UnityEngine.Random.Range(0f, 100f) <= 50f)
        {
            vcams[v].gameObject.SetActive(true);
        }
        else
        {
            vcams[v].gameObject.SetActive(false);
        }
    }
}
