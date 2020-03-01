using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    private NavMeshAgent thisAgent;
    private Health agentHealth;
    private EnemyShooting shootingScript;
    private float patrolRadius = 300.0f;
    private float patrolTimer = 4.0f;

    private float timerCount;

    public PatrolState(AICharacter agent, string reasonForChange) : base(agent, reasonForChange)
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
            if (agentHealth.getCurrentHP >= agentHealth.getBaseHP / 4)
            {
                shootingScript._hasLostTgt = false;
                agent.SetState(new ChaseState(agent, " chasing my attacker either due to HP >= 25%"));
            }
            else
            {
                agent.SetState(new RetreatState(agent, " not enough HP to merit target pursuit or I'm drunk and illogical"));
            }
        }

        Patrol();

        //PlayAnimations();
    }


    public override void FixedTick()
    {

    }

    private bool UnderAttack()
    {
        Health myAttacker = null;

        if (agentHealth._myAttacker != null)
        {
            myAttacker = agentHealth._myAttacker.GetComponent<Health>();

            if (myAttacker != null && agent.getValidTargets.Contains(myAttacker) == false)
            {
                agent.AddTgtToSuperList(myAttacker);

                return true;
            }
            else if (myAttacker != null)
            {
                return true;
            }
        }

        return false;
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
        //Debug.Log("Entered Patrol state");
        thisAgent = agent.GetComponent<NavMeshAgent>();
        agentHealth = agent.GetComponent<Health>();
        shootingScript = agent.GetComponent<EnemyShooting>();

        thisAgent.SetDestination(agent.transform.position);

        agentHealth._myAttacker = null;
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
