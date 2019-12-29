using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveSelfAfterTime : MonoBehaviour
{
    public float afterSec = 0.5f;

    void Start()
    {
        Destroy(gameObject, afterSec);
    }
}
