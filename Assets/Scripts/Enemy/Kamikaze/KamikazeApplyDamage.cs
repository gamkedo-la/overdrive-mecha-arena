using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeApplyDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int blastMaxAffectedObjects;
    [SerializeField] private float blastRadius;
    private FollowPlayer followPlayer;
    private new Transform transform;

    private void Awake()
    {
        transform = base.transform;
        followPlayer = GetComponent<FollowPlayer>();
    }

    private void OnCollisionEnter(Collision _collision)
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
