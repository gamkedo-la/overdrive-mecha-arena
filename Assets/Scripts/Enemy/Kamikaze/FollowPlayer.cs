using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [HideInInspector] public Transform target;

    [SerializeField] private float speed;
    [SerializeField] private float stopFollowingRange;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (target == null)
            return;

        if (Vector3.Distance(target.position, rb.position) < stopFollowingRange)
        {
            Vector3 direction = target.position - rb.position;
            direction = direction.normalized;

            rb.velocity = direction * speed * Time.deltaTime;
        }
        else
            target = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, stopFollowingRange);
    }
}
