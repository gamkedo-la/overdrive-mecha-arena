using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    private NavMeshAgent thisAgent;

    [SerializeField] private float patrolRadius = 100.0f;
    [SerializeField] private float patrolTimer = 10.0f;

    private float timerCount;

    public PatrolState(Character character) : base(character)
    {
    }

    public override void Tick()
    {
        Patrol();

        //if attacked or if found an enemy
        //SetState to ChaseState
    }

    private void Patrol()
    {
        timerCount += Time.deltaTime;
        if (timerCount >= patrolTimer)
        {
            SetNewRandomDest();
            timerCount = 0f;
        }
    }

    public override void OnStateEnter()
    {
        thisAgent = character.GetComponent<NavMeshAgent>();

        timerCount = patrolTimer;
    }

    private void SetNewRandomDest()
    {
        Vector3 newDest = RandomNavSphere(character.transform.position, patrolRadius, -1);
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
