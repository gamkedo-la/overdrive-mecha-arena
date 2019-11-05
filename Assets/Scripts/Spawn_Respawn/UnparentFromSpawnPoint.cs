using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentFromSpawnPoint : MonoBehaviour
{
    void Start()
    {
        transform.parent = null;
    }
}
