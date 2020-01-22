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

    public RetreatState(AICharacter agent) : base(agent)
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
        mech = agent._mech;

        enemyPos = health._myAttacker;

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

    public override void FixedTick()
    {

    }

    public override void Tick()
    {
        enemyPos = health._myAttacker;

        // Determine which type of mech this AI is
        // Semi-randomly select whether this AI should run away or stand and fight (mech type will play a role in this behavior)
        // Execute retreat specific functions (again, depending on the AI's mech type semi-randomly choose whether how it run away or fight; 
        // should it use it's shield, should it just run, should its special move be used)

        if (!isRunningAway)
        {
            RunAway();
        }
        else
        {
            // AI is running away and is still in contact with attacker or a new attacker enters the mix
            // Do I continue to run away, face my current attacker, or self-destruct?
        }

        CheckForSuccessfulRetreatViaDistance();
    }

    private void RunAway()
    {
        if (enemyPos.CompareTag("Player") && FindValidRetreatPoint(playerShooting._playerShootingRange + 150f))
        {
            navAgent.SetDestination(finalRetreatPos);
        }
        else if (enemyPos.CompareTag("Enemy") && !enemyPos.CompareTag("Non-playables") && FindValidRetreatPoint(aiAttackerShootingScript.getBreakContactRange + 150f))
        {
            navAgent.SetDestination(finalRetreatPos);
        }
        else
        {
            Debug.Log(agent.gameObject.name + " cannot retreat due to null attacker or invalid retreat position!");
        }
    }

    private void CheckForSuccessfulRetreatViaDistance()
    {
        var distance = Vector3.Distance(agent.transform.position, enemyPos.position);

        if ((aiAttackerShootingScript != null && distance >= aiAttackerShootingScript.getBreakContactRange && enemyPos.CompareTag("Enemy") && !enemyPos.CompareTag("Non-playables")) ||
            (enemyPos.CompareTag("Player") && distance >= playerShooting._playerShootingRange))
        {
            agent.SetState(new PatrolState(agent));
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
