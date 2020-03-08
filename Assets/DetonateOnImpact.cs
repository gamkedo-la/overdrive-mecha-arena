using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonateOnImpact : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int blastMaxAffectedObjects;
    [SerializeField] private float blastRadius;

    private Collider[] blastColliders;

    private void Start()
    {
        blastColliders = new Collider[blastMaxAffectedObjects];
    }

    private void OnCollisionEnter(Collision collision)
    {
        Physics.OverlapSphereNonAlloc(transform.position, blastRadius, blastColliders);

        for (int i = 0; i < blastColliders.Length; i++)
        {
            if (blastColliders[i].GetComponent<Health>() != null)
            {
                blastColliders[i].GetComponent<Health>().TakeDamage(damage, transform);
            }
        }

        Destroy(gameObject);
    }
}
