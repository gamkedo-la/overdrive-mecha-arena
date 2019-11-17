using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    private List<Health> validTargets;

    private Transform targetTransform;
    private Health currentTgt;

    private NavMeshAgent thisAgent;
    private EnemyShooting shootingScript;
    private Health thisAgentHealth;

    private float dashSpeed = 100.0f;
    // dashTimeLimit not implemented yet but should be used to limit dashing ability to prevent infinite dash
    private float dashTimeLimit = 5.0f;
    private float defaultAiSpeed = 50.0f;

    private float minRangeBeforeDashAllowed = 150.0f;

    private int highValueTgts, midValueTgts, lowValueTgts = 0;
    private float thisAgentPriorityScore;

    public ChaseState(AICharacter agent) : base(agent)
    {
    }

    public override string StateName()
    {
        return "chase state";
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
        if(validTargets.Count == 0)
        {
            // We have no targets so go back to patrolling the arena
            agent.SetState(new PatrolState(agent));
        }

        if (targetTransform != null)
        {
            ChaseTarget();
            shootingScript.FireWeapon(currentTgt);
        }

        PlayAnimations();
        DrawDebugLines();
    }

    private void SelectTarget()
    {
        validTargets = agent.getValidTargets;
        List<Health> toRemove = new List<Health>();

        validTargets.RemoveAll(item => item == null);

        float bestTgtScore = 10000.0f;

        foreach (Health tgt in validTargets)
        {
            if (tgt.getCurrentHP < 0)
            {
                toRemove.Add(tgt);
                continue;
            }

            var distance = Vector3.Distance(agent.transform.position, tgt.transform.position);
            var thisTgtScore = distance + tgt.getPriorityScore;

            if (thisTgtScore <= bestTgtScore)
            {
                targetTransform = tgt.transform;
                currentTgt = tgt;
                bestTgtScore = thisTgtScore;
            }
        }

        foreach (Health tgt in toRemove)
        {
            agent.RemoveTargetFromSuperList(tgt);
        }
    }

    private void ChaseTarget()
    {
        var distance = Vector3.Distance(agent.transform.position, targetTransform.position);

        // only chase target if it's still withing firing range
        if (distance < shootingScript.getBreakContactRange)
        {
            if (distance > minRangeBeforeDashAllowed)
            {
                thisAgent.speed = dashSpeed;
            }
            else
            {
                thisAgent.speed = defaultAiSpeed;
            }
        }
        else
        {
            agent.SetState(new PatrolState(agent));
        }

        // improve set destination so it strafes around its target in a semi-random manner
        thisAgent.SetDestination(targetTransform.position);

        //TODO: polish LookAt code so it's more natural and less instantanious
        // NOTE: Since the GO consists of several parts stitched together in Blender we could also make specific body parts look at a position through code
        agent.transform.LookAt(targetTransform.position);
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

    private void DrawDebugLines()
    {
        foreach (Health tgt in validTargets)
        {
            Debug.DrawLine(agent.transform.position + Vector3.up * 12.0f, tgt.transform.position, Color.yellow);
        }
        if (targetTransform != null)
        {
            Debug.DrawLine(agent.transform.position + Vector3.up * 24.0f, targetTransform.position, Color.red);
        }
    }
}
