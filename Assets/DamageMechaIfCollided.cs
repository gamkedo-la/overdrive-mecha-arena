using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMechaIfCollided : MonoBehaviour
{
    private Rigidbody beamRB;

    [SerializeField] private int beamDamage = 20;
    [SerializeField] private float fallRate = 20.0f;

    void Start()
    {
        beamRB = GetComponent<Rigidbody>();
        Physics.IgnoreLayerCollision(12, 13);
    }
    private void Update()
    {
        //add downward force to beam
        beamRB.AddForce(-transform.up * fallRate);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //first make sure to ignore trigger
        //check if other collider/rigidbody has health and if our beam's y pos is greater than the mechas (y pos + their mesh height)
        //if those are true then we know it can be damaged so call their TakeDamage method
        //now destroy the beam GO
        //else do nothing and destroy the beam GO since it's useless
    }
}
