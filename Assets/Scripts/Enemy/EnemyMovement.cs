using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private EnemyAggro aggroDetection;
    private NavMeshAgent navMeshAgent;
    private Transform target;

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
        if (target != null)
        {
            navMeshAgent.SetDestination(target.position);
        }
    }
}
