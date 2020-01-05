using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttacker : MonoBehaviour
{
    public int damage = 1;

    private void OnParticleCollision(GameObject other)
    {
        // Debug.Log("particle hit: " + other.name);
        Health mechaHealth = other.gameObject.GetComponent<Health>();

        if (mechaHealth != null)
        {
            // Debug.Log("Mecha hit by dangerous particle!");
            mechaHealth.TakeDamage(damage);
        }
    }
}
