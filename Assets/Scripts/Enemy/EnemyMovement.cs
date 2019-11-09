using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private EnemyAggro aggroDetection;
    private NavMeshAgent navMeshAgent;
    private Transform target;

    [SerializeField] private float dashSpeed = 100.0f;
    [SerializeField] private float minRangeBeforeDashAllowed = 100.0f;
    // dashTimeLimit not implemented yet but should be used to limit dashing ability to prevent infinite dash
    [SerializeField] private float dashTimeLimit = 5.0f;
    private float defaultAiSpeed = 50.0f;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        aggroDetection = GetComponent<EnemyAggro>();
        aggroDetection.Aggroed += AggroDetection_Aggroed;
    }

    private void AggroDetection_Aggroed(Transform target)
    {
        this.target = target;
    }

    private void Update()
    {
        //Debug.Log(gameObject.name + " targeting " + target.name);
        if (target != null)
        {
            ChaseTarget();
        }
    }

    private void ChaseTarget()
    {
        //check distance between target and this AI
        // if distance is greater than say 100.0f then set navMeshAgent speed to it current value times the dash speed
        var distance = Vector3.Distance(transform.position, target.position);

        // TODO: break this off into function after programming AI logic for it to determine whether dashing is worth using or if choosing a new target is better
        if(distance > minRangeBeforeDashAllowed)
        {
            navMeshAgent.speed = dashSpeed;
        }
        else
        {
            navMeshAgent.speed = defaultAiSpeed;
        }

        navMeshAgent.SetDestination(target.position);
        //TODO: polish LookAt code so it's more natural and less instantanious
        transform.LookAt(target.position);
    }
}
