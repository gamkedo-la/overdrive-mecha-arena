using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    private NavMeshAgent thisAgent;

    private float patrolRadius = 100.0f;
    private float patrolTimer = 10.0f;

    private float timerCount;

    public PatrolState(AICharacter agent) : base(agent)
    {
    }

    public override void Tick()
    {
        Patrol();
        Observe();

        //if attacked or if found an enemy
        //SetState to ChaseState
    }

    private void Observe()
    {
        //Debug.Log("Observing for threats");
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
        thisAgent = agent.GetComponent<NavMeshAgent>();

        timerCount = patrolTimer;
    }

    private void SetNewRandomDest()
    {
        Vector3 newDest = RandomNavSphere(agent.transform.position, patrolRadius, -1);
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
