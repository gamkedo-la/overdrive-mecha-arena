using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyVision : MonoBehaviour
{

    public Transform player;
    public Vector3 playerLastPosition;
    public GameObject playerObject;
    public Transform enemyTransform;
    public float turnSpeed = 0.1f;
    public float attackRange;
    public float fieldOfViewAngle = 110f;
    public bool isAttacking = false;
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
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.transform;
        agent = GetComponentInParent<NavMeshAgent>();
        isDying = GetComponentInParent<enemyScript>().enemyIsDying;
    }

    public void activateAttack()
    {
        isAttacking = true;
    }
    public void deactivateAttack()
    {
        isAttacking = false;
    }

    void Update()
    {

        isDying = GetComponentInParent<enemyScript>().enemyIsDying;
        if (!isDying)
        {

            if (hasBeenShot == true)
            {
                agent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
                playerLastPosition = player.position;
                hasSeenPlayer = true;
            }

            RaycastHit hit;
            Vector3 targetPt = player.position + Vector3.up * 18.0f; // projecting off feet
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
                Debug.Log(visionConeSeesPlayer + " " + nearAwarenessNoticesPlayer);
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position + direction.normalized * visionRange, Color.red);
            }

            if (visionConeSeesPlayer || nearAwarenessNoticesPlayer)
            {
                playerLastPosition = targetPt;
                agent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
                hasBeenShot = false;

                direction.y = 0;
                transform.parent.transform.rotation = Quaternion.Slerp(transform.parent.transform.rotation,
                                            Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);

                if (Vector3.Distance(targetPt, transform.position) > attackRange)
                {

                    this.agent.SetDestination(player.position);
                    playerLastPosition = hit.point;
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
                    float distanceToTarget = Vector3.Distance(playerLastPosition, enemyTransform.transform.position);
                    float distanceThreshold = .6f;
                    //if (distanceToTarget > distanceThreshold)
                    //{
                    //    agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                    //    //Debug.Log("DistToTarget " + distanceToTarget);
                    //    //Debug.Log("DistToThresh " + distanceThreshold);
                    //    Debug.Log("Searching for player at last position");
                    //    this.agent.SetDestination(playerLastPosition);
                    //}
                    //else if (distanceToTarget <= distanceThreshold || agent.velocity == Vector3.zero)
                    //{
                    //    // Debug.Log("At last player position");
                    //    this.agent.SetDestination(enemyTransform.position);
                    //    agent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
                    //    hasSeenPlayer = false;
                    //}

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
            //spawnCloud.Play();
        }

    }

    private void SearchForTarget()
    {
        //Debug.Log("Searching for target");
        this.agent.SetDestination(playerLastPosition);
    }

    void OnTriggerEnter(Collider collision)
    {

        /*if (collision.gameObject.tag == "Player")
         {
             agent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
             playerLastPosition = player.position;
             hasSeenPlayer = true;
         }*/
    }
}

