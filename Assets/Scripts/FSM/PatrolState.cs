using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    private NavMeshAgent thisAgent;

    private float patrolRadius = 300.0f;
    private float patrolTimer = 5.0f;

    private float timerCount;

    private bool takingFire = false;

    public PatrolState(AICharacter agent) : base(agent)
    {
    }

    public override string StateName()
    {
        return "patrol state";
    }

    public override void Tick()
    {
        if (UnderAttack())
        {
            //SetState to ChaseState
        }

        Patrol();

        PlayAnimations();

        Observe();
    }

    public override void FixedTick()
    {
        
    }

    private bool UnderAttack()
    {
        // check if this AI is taking damage
        // if it is then set takingFire to true
        // else set takingFire to false

        return takingFire;
    }

    private void Observe()
    {
        // Debug.Log("Observing for threats");

        // Have AI choose which look left, right, back, and forward in semi-random fashion
        // Use capsule trigger as "cone of vision" and attack the target if it has a Health script
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
        agent.getValidTargets.Clear();

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

    private void PlayAnimations()
    {
        if (thisAgent.velocity.sqrMagnitude == 0)
        {
            agent.getAnimator.SetFloat("Speed", 0f);
        }
        else
        {
            agent.getAnimator.SetFloat("Speed", 1f);
        }
    }
}
