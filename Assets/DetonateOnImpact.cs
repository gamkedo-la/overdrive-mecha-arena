using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonateOnImpact : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int blastMaxAffectedObjects;
    [SerializeField] private float blastRadius;

    private void OnCollisionEnter(Collision collision)
    {
        var colls = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (var col in colls)
        {
            if (col.CompareTag("Player") || col.CompareTag("Enemy"))
            {
                Health hpTGT = col.GetComponent<Health>();
                if (hpTGT != null)
                {
                    hpTGT.TakeDamage(damage, transform);
                }
            }
        }

        Destroy(gameObject);
    }
}
