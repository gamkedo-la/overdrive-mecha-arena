using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    private Collider otherCol;
    private Health target;
    private Transform targetTransform;
    private NavMeshAgent thisAgent;
    private EnemyShooting shootingScript;

    private float dashSpeed = 100.0f;
    private float minRangeBeforeDashAllowed = 100.0f;
    // dashTimeLimit not implemented yet but should be used to limit dashing ability to prevent infinite dash
    private float dashTimeLimit = 5.0f;
    private float defaultAiSpeed = 50.0f;

    public ChaseState(AICharacter agent) : base(agent)
    {
    }

    public override void Tick()
    {
        ChaseTarget();
        shootingScript.FireWeapon();
    }

    private void ChaseTarget()
    {
        var distance = Vector3.Distance(agent.transform.position, targetTransform.position);

        // TODO: implement AI choice to use dash or save it depending on target health and distance
        if (distance > minRangeBeforeDashAllowed)
        {
            thisAgent.speed = dashSpeed;
        }
        else
        {
            thisAgent.speed = defaultAiSpeed;
        }

        thisAgent.SetDestination(targetTransform.position);

        //TODO: polish LookAt code so it's more natural and less instantanious
        agent.transform.LookAt(targetTransform.position);
    }

    public override void OnStateEnter()
    {
        thisAgent = agent.GetComponent<NavMeshAgent>();
        shootingScript = agent.getShootingScript;
        otherCol = agent.getTargetCollider;
        target = otherCol.GetComponent<Health>();

        if (target != null)
        {
            targetTransform = target.transform;
        }
    }
}
