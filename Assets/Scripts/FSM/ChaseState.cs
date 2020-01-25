using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    private List<Health> validTargets;
    private List<Health> nearTargets = new List<Health>();

    private Transform targetTransform;
    private Health currentTgt;

    private NavMeshAgent thisAgent;
    private EnemyShooting shootingScript;
    private Health thisAgentHealth;

    private float dashSpeed = 500.0f;
    // dashTimeLimit not implemented yet but should be used to limit dashing ability to prevent infinite dash
    private float dashTimeLimit = 5.0f;
    private float defaultAiSpeed = 100.0f;

    private float minRangeBeforeDashAllowed = 150.0f;
    private float retreatDistance = 80.0f;

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

        dashSpeed = agent._mech.dashSpeed;
        dashTimeLimit = agent._mech.dashTimeLimit;
        defaultAiSpeed = agent._mech.fowardMoveSpeed;
        minRangeBeforeDashAllowed = agent._mech.range / 2;
    }

    public override void Tick()
    {
        if (thisAgentHealth.getCurrentHP <= (thisAgentHealth.getBaseHP / 3) && 
            !agent.gameObject.CompareTag("Non-playables") && 
            !thisAgentHealth._myAttacker.CompareTag("Hazards") &&
            thisAgentHealth._myAttacker != null)
        {
            //Debug.Log(agent.gameObject.name + " is entering Retreat State");
            agent.SetState(new RetreatState(agent));
        }

        SelectTarget();
        if (validTargets.Count == 0)
        {
            // We have no targets so go back to patrolling the arena
            agent.SetState(new PatrolState(agent));
        }

        shootingScript.isTryingToDash = false; // ChaseTarget may override on frame by frame basis

        if (targetTransform != null)
        {
            ChaseTarget();
        }

        PlayAnimations();
        DrawDebugLines();
    }

    public override void FixedTick()
    {
        if (targetTransform != null)
        {
            shootingScript.FireWeapon(currentTgt, defaultAiSpeed, dashSpeed, ShouldUseOverdrive());

            agent.transform.LookAt(targetTransform.position);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, Quaternion.LookRotation(targetTransform.position - agent.transform.position), 0.05f);
        }
    }

    private bool ShouldUseOverdrive()
    {
        return nearTargets.Count >= 4;
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

            if(distance <= minRangeBeforeDashAllowed)
            {
                if(!nearTargets.Contains(tgt))
                    nearTargets.Add(tgt);
            }
            else
            {
                nearTargets.Remove(tgt);
            }

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

        //Debug.Log(distance);

        // only chase target if it's still withing firing range
        if (distance < shootingScript.getBreakContactRange)
        {
            shootingScript.isTryingToDash = distance > minRangeBeforeDashAllowed;

            if (shootingScript.isTryingToDash)
            {
                thisAgent.speed = defaultAiSpeed * dashSpeed;
            }
            else
            {
                thisAgent.speed = defaultAiSpeed;
            }

            //TODO: polish LookAt code so it's more natural and less instantanious
            // NOTE: Since the GO consists of several parts stitched together in Blender we could also make specific body parts look at a position through code

            if (distance > thisAgent.stoppingDistance)
            {
                thisAgent.SetDestination(targetTransform.position);
            }
            else if (distance < retreatDistance)
            {
                Vector3 toTgt = targetTransform.position - agent.transform.position;
                Vector3 tgtPos = toTgt.normalized * -200f + targetTransform.position;

                thisAgent.SetDestination(tgtPos);


                if (agent.debugPoint)
                {
                    agent.debugPoint.position = tgtPos;
                }

            }

            // improve set destination so it strafes around its target in a semi-random manner
            //thisAgent.SetDestination(targetTransform.position);
        }
        else
        {
            //Debug.Log("Entering Patrol State");
            agent.SetState(new PatrolState(agent));
        }
    }

    private void PlayAnimations()
    {
        if (agent.getAnimator == null)
        {
            Debug.LogWarning("animator not found for agent: " + thisAgent.name);
            return;
        }
        if (thisAgent.velocity.sqrMagnitude == 0)
        {
            agent.getAnimator.SetFloat("Speed", 0f);
        }
        else
        {
            agent.getAnimator.SetFloat("Speed", 1f);
        }

        //Debug.Log(thisAgent.velocity.sqrMagnitude);
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
