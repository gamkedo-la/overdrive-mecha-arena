using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyVision : MonoBehaviour
{

    public Transform target;
    public Vector3 tgtLastPosition;
    public GameObject tgtObject;
    public Transform enemyTransform;

    public float turnSpeed = 0.1f;
    public float attackRange;
    public float fieldOfViewAngle = 110f;

    public bool isDying = false;
    public bool hasBeenShot = false;
    public bool hasSeenPlayer = false;

    public Vector3 personalLastSighting;

    private SphereCollider col;
    public NavMeshAgent agent;

    public int awarenessRange = 20;
    public float visionRange = 100.0f;

    private bool isSearching = false;

    void Start()
    {
        tgtObject = GameObject.FindGameObjectWithTag("Player");
        target = tgtObject.transform;
        agent = GetComponentInParent<NavMeshAgent>();
    }

    void Update()
    {

        isDying = GetComponentInParent<Health>().IsDying();
        if (isDying == false)
        {

            if (hasBeenShot == true)
            {
                agent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
                tgtLastPosition = target.position;
                hasSeenPlayer = true;
            }

            RaycastHit hit;
            Vector3 targetPt = target.position + Vector3.up * 18.0f; // projecting off feet
            Vector3 direction = targetPt - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            bool didRayHitPlayer = Physics.Raycast(transform.position, direction, out hit, 1000) && hit.collider.gameObject.tag == "Player";

            bool visionConeSeesPlayer = false;
            bool nearAwarenessNoticesPlayer = false;

            if (didRayHitPlayer)
            {
                Debug.DrawLine(transform.position, hit.point, Color.cyan);
                visionConeSeesPlayer = Vector3.Distance(targetPt, transform.position) < visionRange && angle < fieldOfViewAngle;
                //Debug.Log(Mathf.RoundToInt(Vector3.Distance(targetPt, transform.position)) + " " + Mathf.RoundToInt(angle));
                nearAwarenessNoticesPlayer = Vector3.Distance(targetPt, transform.position) < awarenessRange;
                //Debug.Log(visionConeSeesPlayer + " " + nearAwarenessNoticesPlayer);
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position + direction.normalized * visionRange, Color.red);
            }

            if (visionConeSeesPlayer || nearAwarenessNoticesPlayer)
            {
                tgtLastPosition = targetPt;
                agent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
                hasBeenShot = false;

                direction.y = 0;
                transform.parent.transform.rotation = Quaternion.Slerp(transform.parent.transform.rotation,
                                            Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);

                if (Vector3.Distance(targetPt, transform.position) > attackRange)
                {

                    this.agent.SetDestination(target.position);
                    tgtLastPosition = hit.point;
                    // Debug.Log("Last player position" + playerLastPosition);
                    hasSeenPlayer = true;
                }

                else
                {
                    this.agent.SetDestination(enemyTransform.position);
                }
            }
            else
            {
                if (hasSeenPlayer == true)
                {
                    //Debug.Log("PLAYER SEEN GO TO PLAYER");
                    // awarenessRange = 5;
                    float distanceToTarget = Vector3.Distance(tgtLastPosition, enemyTransform.transform.position);
                    float distanceThreshold = .6f;
                    if (distanceToTarget > distanceThreshold)
                    {
                        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                        //Debug.Log("DistToTarget " + distanceToTarget);
                        //Debug.Log("DistToThresh " + distanceThreshold);
                        Debug.Log("Searching for player at last position");
                        this.agent.SetDestination(tgtLastPosition);
                    }
                    else if (distanceToTarget <= distanceThreshold || agent.velocity == Vector3.zero)
                    {
                        // Debug.Log("At last player position");
                        this.agent.SetDestination(enemyTransform.position);
                        agent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
                        hasSeenPlayer = false;
                    }

                    //SearchForTarget();
                }
            }

            if (!didRayHitPlayer && hasSeenPlayer)
            {
                SearchForTarget();
            }

        }
        else
        {
            this.GetComponent<CapsuleCollider>().enabled = false;
        }

    }

    private void SearchForTarget()
    {
        //Debug.Log("Searching for target");
        this.agent.SetDestination(tgtLastPosition);
    }
}

