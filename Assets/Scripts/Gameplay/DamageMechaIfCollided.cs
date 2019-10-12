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
        //ignore collision between dangerous objects and triggers
        Physics.IgnoreLayerCollision(12, 13);
    }
    private void Update()
    {
        beamRB.AddForce(-transform.up * fallRate);
        if(gameObject != null)
        {
            Destroy(gameObject, 5.0f);
        }
    }

    private void OnCollisionEnter(Collision collider)
    {
        //check if other collider/rigidbody has health and if our beam's y pos is greater than the mechas (y pos + their mesh height)
        //if those are true then we know it can be damaged so call their TakeDamage method
        //now destroy the beam GO
        //else do nothing and destroy the beam GO since it's useless

        //next code block is placeholder code
        Health mechaHealth = collider.gameObject.GetComponent<Health>();

        if(mechaHealth != null)
        {
            //Debug.Log("Mecha hit by beam!");
            mechaHealth.TakeDamage(beamDamage);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Collider that made contact with me does not have a Health script!");
        }//end of placeholder code
    }
}
