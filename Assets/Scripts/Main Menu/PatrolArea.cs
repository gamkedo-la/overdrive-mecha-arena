using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolArea : MonoBehaviour
{
    private NavMeshAgent thisAgent;

    [SerializeField] private float patrolRadius = 100.0f;
    [SerializeField] private float patrolTimer = 10.0f;

    private float timerCount;

    private void Awake()
    {
        thisAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        timerCount = patrolTimer;
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        timerCount += Time.deltaTime;
        if(timerCount >= patrolTimer)
        {
            SetNewRandomDest();
            timerCount = 0f;
        }
    }

    private void SetNewRandomDest()
    {
        Vector3 newDest = RandomNavSphere(transform.position, patrolRadius, -1);
        thisAgent.SetDestination(newDest);
    }

    private Vector3 RandomNavSphere(Vector3 origin, float patrolRadius, int layerMask)
    {
        Vector3 randDir = UnityEngine.Random.insideUnitSphere * patrolRadius;
        randDir += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDir, out navHit, patrolRadius, layerMask);

        return navHit.position;
    }
}
