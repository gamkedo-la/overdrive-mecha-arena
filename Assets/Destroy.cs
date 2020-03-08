using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] private float time;

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, time);
    }
}
