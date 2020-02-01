using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetName : MonoBehaviour
{
    [SerializeField] private Mecha mech;

    private void Start()
    {
        gameObject.name = mech.names[Random.Range(0, mech.names.Count - 1)];
    }
}
