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
    private Collider[] blastColliders;

    private void Awake()
    {
        transform = base.transform;
        followPlayer = GetComponent<FollowPlayer>();
    }

    private void Start()
    {
        blastColliders = new Collider[blastMaxAffectedObjects];
    }

    private void OnCollisionEnter(Collision _collision)
    {
        Physics.OverlapSphereNonAlloc(transform.position, blastRadius, blastColliders);

        for (int i = 0; i < blastColliders.Length; i++)
            blastColliders[i].GetComponent<Health>()?.TakeDamage(damage, transform);

        Destroy(gameObject);
    }
}
