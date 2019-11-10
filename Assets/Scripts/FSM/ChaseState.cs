using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    private List<Health> validTargets;

    private Transform targetTransform;
    private NavMeshAgent thisAgent;
    private EnemyShooting shootingScript;
    private Health thisAgentHealth;

    private float dashSpeed = 100.0f;
    private float minRangeBeforeDashAllowed = 100.0f;
    // dashTimeLimit not implemented yet but should be used to limit dashing ability to prevent infinite dash
    private float dashTimeLimit = 5.0f;
    private float defaultAiSpeed = 50.0f;

    private int highValueTgts, midValueTgts, lowValueTgts = 0;
    private float nearThreatDistance = 100.0f;

    public ChaseState(AICharacter agent) : base(agent)
    {
    }

    public override void OnStateEnter()
    {
        thisAgent = agent.GetComponent<NavMeshAgent>();
        shootingScript = agent.GetComponent<EnemyShooting>();
        thisAgentHealth = agent.GetComponent<Health>();
    }

    public override void Tick()
    {
        SelectTarget();

        if(targetTransform != null)
        {
            ChaseTarget();
            shootingScript.FireWeapon();
        }
    }

    private void SelectTarget()
    {
        // cycle through valid targets
        // determine how many high, medium, low value targets there
        // determine this agent's target value
        // compared this agent's tgt value to the number of highs, mediums, and lows
        // find distance between this agent and target
        // use the target's priority, distance, and this agent's target value to decide which target to pursue

        validTargets = agent.getValidTargets;
        string thisAgentTargetValue = thisAgentHealth.getTargetPriority;

        foreach (Health tgt in validTargets)
        {
            var distance = Vector3.Distance(agent.transform.position, tgt.transform.position);
            if(distance <= nearThreatDistance)
            {
                targetTransform = tgt.transform;
            }
        }
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
}
