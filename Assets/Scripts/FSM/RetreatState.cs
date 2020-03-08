using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RetreatState : State
{
    private Mecha mech;
    private Transform enemyPos;//AI wants to get away from it's attacker
    private Health health;

    private EnemyShooting thisAgentShootingScript;
    private EnemyShooting aiAttackerShootingScript;
    private PlayerShooting playerShooting;

    private NavMeshAgent navAgent;

    private bool isRunningAway = false;
    private bool isFightingBack = false;

    private float minDistanceFromEnemyPos;
    private Vector3 finalRetreatPos;

    private float defaultAiSpeed = 100.0f;
    private DoubleStatsSpecial doubleStats;

    public RetreatState(AICharacter agent, string reasonForChange) : base(agent, reasonForChange)
    {
    }
    public override string StateName()
    {
        return "retreat state";
    }

    public override void OnStateEnter()
    {
        //Debug.Log(agent.gameObject.name + " is entered Retreat State");

        navAgent = agent.GetComponent<NavMeshAgent>();
        thisAgentShootingScript = agent.GetComponent<EnemyShooting>();
        health = agent.GetComponent<Health>();
        doubleStats = agent.GetComponent<DoubleStatsSpecial>();

        if (doubleStats != null && doubleStats._areStatsBuffed)
        {
            defaultAiSpeed = agent._mech.fowardMoveSpeed * 2;
        }
        else
        {
            defaultAiSpeed = agent._mech.fowardMoveSpeed;
        }

        navAgent.SetDestination(agent.transform.position);

        mech = agent._mech;
        enemyPos = health._myAttacker;

        if (enemyPos != null)
        {
            if (enemyPos.CompareTag("Player"))
            {
                playerShooting = enemyPos.GetComponent<PlayerShooting>();
                minDistanceFromEnemyPos = playerShooting._playerShootingRange;
            }
            else if (enemyPos.CompareTag("Enemy") && !enemyPos.CompareTag("Non-playables"))
            {
                aiAttackerShootingScript = enemyPos.GetComponent<EnemyShooting>();
                minDistanceFromEnemyPos = aiAttackerShootingScript.getBreakContactRange;
            }
            else
            {
                Debug.LogError(agent.gameObject.name + "'s " + "attacker is null or unknown!");
            }
        }
    }

    public override void FixedTick()
    {

    }

    public override void Tick()
    {
        if (enemyPos != null && enemyPos.CompareTag("Non-playables") == false)
        {
            if (doubleStats != null && doubleStats._areStatsBuffed)
            {
                defaultAiSpeed = agent._mech.fowardMoveSpeed * 2;
                navAgent.speed = defaultAiSpeed;
            }
            else
            {
                defaultAiSpeed = agent._mech.fowardMoveSpeed;
                navAgent.speed = defaultAiSpeed;
            }

            if (!isRunningAway)
            {
                //Debug.Log(agent.gameObject.name + " is attempting to run away");
                RunAway();
            }
            else
            {
                EnterPatrolIfRetreatViaDistanceSucceeds();

                // AI is running away and is still in contact with attacker or a new attacker enters the mix
                // Do I continue to run away, face my current attacker, or self-destruct?
            }
        }
        else
        {
            Vector3 newDest = RandomNavSphere(agent.transform.position, 300f, -1);
            navAgent.SetDestination(newDest);

            agent.SetState(new PatrolState(agent, " there's no enemy attacking me or I'm being attacked by a dangerous object"));
        }
    }

    private Vector3 RandomNavSphere(Vector3 origin, float patrolRadius, int layerMask)
    {
        Vector3 randDir = UnityEngine.Random.insideUnitSphere * patrolRadius;
        randDir += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDir, out navHit, patrolRadius, layerMask);

        return navHit.position;
    }

    private void RunAway()
    {
        if (enemyPos.CompareTag("Player") && FindValidRetreatPoint(playerShooting._playerShootingRange + 150f))
        {
            navAgent.SetDestination(finalRetreatPos);
        }
        else if (enemyPos.CompareTag("Enemy") && FindValidRetreatPoint(aiAttackerShootingScript.getBreakContactRange + 150f))
        {
            navAgent.SetDestination(finalRetreatPos);
        }
        else
        {
            Debug.Log(agent.gameObject.name + " cannot retreat due to invalid retreat position!");
        }
    }

    private void EnterPatrolIfRetreatViaDistanceSucceeds()
    {
        var distance = Vector3.Distance(agent.transform.position, enemyPos.position);

        if ((aiAttackerShootingScript != null && distance >= aiAttackerShootingScript.getBreakContactRange && enemyPos.CompareTag("Enemy") ||
            (enemyPos.CompareTag("Player") && distance >= playerShooting._playerShootingRange)))
        {
            health._myAttacker = null;
            agent.SetState(new PatrolState(agent, " succeeded with retreat"));
        }
    }


    public bool FindValidRetreatPoint(float radius)
    {
        //Debug.Log("Attempting to find a valid retreat point");

        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += agent.transform.position;
        NavMeshHit hit;
        finalRetreatPos = Vector3.zero;

        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            var distance = Vector3.Distance(hit.position, enemyPos.position);
            //Debug.Log("The distance between my attacker and my retreat point is: " + distance);

            if (distance < minDistanceFromEnemyPos)
            {
                isRunningAway = false;
                return false;
            }
            else
            {
                //Debug.Log("Found a valid retreat point");
                isRunningAway = true;
                finalRetreatPos = hit.position;
                return true;
            }
        }
        else
        {
            isRunningAway = false;
        }

        return false;
    }
}
