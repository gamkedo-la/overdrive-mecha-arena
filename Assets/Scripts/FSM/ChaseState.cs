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
    private Transform lastTgtPos;
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

    private float minStoppingDist = 115.0f;
    private float maxStoppingDist;
    private float stoppingDistRandom;

    private float chooseNewStopDistTimer = 10.0f;
    private float newStopDistTimerMax = 15.0f;
    private float newStopDistTimerMin = 5.0f;
    private float randomNewDistTimer;

    private float thisAgentPriorityScore;

    private float targetUpdateDelay = 20f;
    private float targetUpdateTimer = 0f;
    private bool selectedInitialTgt = false;
    private float drunkTargetUpdateDelay = 10.0f;

    private float timeUntilStrafe = 2.5f;
    private int randomStrafeDir;
    private float minTimeTillStrafe = 2.0f;
    private float maxTimeTillStrafe = 6.0f;
    private float randomStrafeTime;
    private float drunkStrafeTimer = 10.0f;

    private DoubleStatsSpecial doubleStats;

    public ChaseState(AICharacter agent, string reasonForChange) : base(agent, reasonForChange)
    {
    }

    public override string StateName()
    {
        return "chase state";
    }

    public override void OnStateEnter()
    {
        //Debug.Log("Entered Chase state");
        thisAgent = agent.GetComponent<NavMeshAgent>();
        shootingScript = agent.GetComponent<EnemyShooting>();
        thisAgentHealth = agent.GetComponent<Health>();

        doubleStats = agent.GetComponent<DoubleStatsSpecial>();

        if (doubleStats != null && doubleStats._areStatsBuffed)
        {
            dashSpeed = agent._mech.dashSpeed * 2;
            dashTimeLimit = agent._mech.dashTimeLimit * 2;
            defaultAiSpeed = agent._mech.fowardMoveSpeed * 2;
        }
        else
        {
            dashSpeed = agent._mech.dashSpeed;
            dashTimeLimit = agent._mech.dashTimeLimit;
            defaultAiSpeed = agent._mech.fowardMoveSpeed;
        }

        minRangeBeforeDashAllowed = agent._mech.range / 2;
        maxStoppingDist = agent._mech.range - 20f;

        thisAgent.SetDestination(agent.transform.position);
    }

    public override void Tick()
    {
        if(doubleStats != null && doubleStats._areStatsBuffed)
        {
            dashSpeed = agent._mech.dashSpeed * 2;
            dashTimeLimit = agent._mech.dashTimeLimit * 2;
            defaultAiSpeed = agent._mech.fowardMoveSpeed * 2;
        }
        else
        {
            dashSpeed = agent._mech.dashSpeed;
            dashTimeLimit = agent._mech.dashTimeLimit;
            defaultAiSpeed = agent._mech.fowardMoveSpeed;
        }

        // TODO: bypass target selection delay whenever a new target is added to this AI's valid targets list
        if (!selectedInitialTgt)
        {
            SelectTarget();
        }
        else
        {
            targetUpdateTimer += Time.deltaTime;
            if (!thisAgentHealth.isInOverdrive())
            {
                if (targetUpdateTimer >= targetUpdateDelay)
                {
                    targetUpdateTimer = 0f;
                    //Debug.Log(agent.name + " updating target to best target possible");
                    SelectTarget();
                }
            }
            else if (targetUpdateTimer >= drunkTargetUpdateDelay)
            {
                targetUpdateTimer = 0f;
                //Debug.Log(agent.name + " updating target to best target possible");
                SelectTarget();
            }
        }

        if (validTargets.Count == 0)
        {
            agent.SetState(new PatrolState(agent, " patrolling due to lack of targets"));
        }

        shootingScript.isTryingToDash = false; // ChaseTarget may override on frame by frame basis

        if (targetTransform != null)
        {
            lastTgtPos = targetTransform;

            if (shootingScript._hasLostTgt == false)
            {
                ChaseTarget();
            }
            else
            {
                MoveToLastKnownTgtPos();
            }
        }

        //PlayAnimations();
        DrawDebugLines();
    }

    private void RandomRetreat()
    {
        float random = UnityEngine.Random.Range(0f, 100f);

        if(random <= 10.0f)
        {
            shootingScript._hasLostTgt = true;
            agent.SetState(new RetreatState(agent, " running away (drunk and acting funny)"));
        }
    }

    private void MoveToLastKnownTgtPos()
    {
        //thisAgent.SetDestination(lastTgtPos.position);
        agent.SetState(new PatrolState(agent, " lost target. going to patrol (MoveToLastKnownTgtPos)"));
    }

    public override void FixedTick()
    {
        if (thisAgentHealth.isInOverdrive())
        {
            RandomRetreat();
        }
        else if(thisAgentHealth.getCurrentHP < thisAgentHealth.getBaseHP/4)
        {
            agent.SetState(new RetreatState(agent, " lost too much health"));
        }

        if (targetTransform != null && shootingScript._hasLostTgt == false)
        {
            shootingScript.FireWeapon(currentTgt, defaultAiSpeed, dashSpeed, ShouldUseOverdrive());

            SmoothLookAt();
        }
    }

    private void SmoothLookAt()
    {
        // NOTE: Since the GO consists of several parts stitched together in Blender we could also make specific body parts look at a position through code

        Vector3 lookAtPos = targetTransform.position - agent.transform.position;
        lookAtPos.y = 0;
        Quaternion rot = Quaternion.LookRotation(lookAtPos);
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rot, 0.075f);
    }

    private bool ShouldUseOverdrive()
    {
        return nearTargets.Count >= 2;
    }

    private void SelectTarget()
    {
        //if (currentTgt != null)
        //{
        //    return;
        //}

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

            if (distance <= minRangeBeforeDashAllowed)
            {
                if (!nearTargets.Contains(tgt))
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

        if (currentTgt != null)
        {
            selectedInitialTgt = true;
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

            RandomizeStoppingDist();

            if (distance > thisAgent.stoppingDistance) // target selection criteria is being skewed by this if block since it makes multiple mechs always be within 100ish meters of their tgt
            {
                thisAgent.SetDestination(targetTransform.position);
            }
            else if (distance < retreatDistance)
            {
                MoveBackwards();
            }
            else
            {
                HandleStrafing();
            }
        }
        else
        {
            agent.SetState(new PatrolState(agent, " patrolling due to target being out of range"));
        }
    }

    private void MoveBackwards()
    {
        Vector3 toTgt = targetTransform.position - agent.transform.position;
        Vector3 tgtPos = toTgt.normalized * -200f + targetTransform.position;

        NavMeshHit hit;
        if (NavMesh.FindClosestEdge(tgtPos, out hit, NavMesh.AllAreas))
        {
            thisAgent.SetDestination(hit.position);

            //if (agent.debugPoint)
            //{
            //    agent.debugPoint.position = hit.position;
            //}
        }
        //thisAgent.SetDestination(tgtPos);

        //if (agent.debugPoint)
        //{
        //    agent.debugPoint.position = tgtPos;
        //}
    }

    private void HandleStrafing()
    {
        randomStrafeDir = UnityEngine.Random.Range(0, 2);
        randomStrafeTime = UnityEngine.Random.Range(minTimeTillStrafe, maxTimeTillStrafe);

        if (timeUntilStrafe <= 0)
        {
            if (randomStrafeDir == 0)
            {
                StrafeRightOrLeft(true);
            }
            else
            {
                StrafeRightOrLeft(false);
            }

            if (!thisAgentHealth.isInOverdrive())
            {
                timeUntilStrafe = randomStrafeTime;
            }
            else
            {
                timeUntilStrafe = drunkStrafeTimer;
            }
        }
        else
        {
            timeUntilStrafe -= Time.deltaTime;
        }
    }

    private void RandomizeStoppingDist()
    {
        stoppingDistRandom = UnityEngine.Random.Range(minStoppingDist, maxStoppingDist);
        randomNewDistTimer = UnityEngine.Random.Range(newStopDistTimerMin, newStopDistTimerMax);

        if (chooseNewStopDistTimer <= 0)
        {
            chooseNewStopDistTimer = randomNewDistTimer;
            thisAgent.stoppingDistance = stoppingDistRandom;
        }
        else
        {
            chooseNewStopDistTimer -= Time.deltaTime;
        }
    }

    private void StrafeRightOrLeft(bool strafeRight)
    {
        //Debug.Log(agent.name + " attempting to strafe right? " + strafeRight);

        Vector3 offset;
        if (strafeRight)
        {
            offset = targetTransform.position - agent.transform.position * -200f;
        }
        else
        {
            offset = agent.transform.position - targetTransform.position * -200f;
        }

        var dir = Vector3.Cross(offset, Vector3.up);

        //Debug.Log(agent.transform.position + dir);
        NavMeshHit hit;
        if (NavMesh.FindClosestEdge((agent.transform.position + dir).normalized, out hit, NavMesh.AllAreas))// using .normalized and * -200f in offset assignment seems to works decently
        {
            thisAgent.SetDestination(hit.position);
            //if (agent.debugPoint)
            //{
            //    agent.debugPoint.position = hit.position;
            //}
        }
        //thisAgent.SetDestination(agent.transform.position + dir);
        //if (agent.debugPoint)
        //{
        //    agent.debugPoint.position = hit.position;
        //}
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
