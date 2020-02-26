using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightToMech : MonoBehaviour
{
    [SerializeField] private List<Transform> mechs;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mechs[1].transform);
    }
}
