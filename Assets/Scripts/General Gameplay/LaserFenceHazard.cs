using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFenceHazard : MonoBehaviour
{
    [SerializeField] private int damage = 20;

    void Start()
    {
        //ignore collision between dangerous objects and triggers
        Physics.IgnoreLayerCollision(12, 13);
    }

    private void OnTriggerEnter(Collider other)
    {
        Health mechaHealth = other.gameObject.GetComponent<Health>();

        if (mechaHealth != null)
        {
            Debug.Log("Mecha hit by dangerous object!");
            mechaHealth.TakeDamage(damage);
        }
        else
        {
            Debug.Log("Collider that made contact with me does not have a Health script!");
        }//end of placeholder code
    }
}
