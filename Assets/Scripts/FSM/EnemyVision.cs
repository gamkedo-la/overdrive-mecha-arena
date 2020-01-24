using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyVision : AICharacter
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

    public int awarenessRange = 20;
    public float visionRange = 100.0f;

    void Start()
    {
        // need to find a way to set this line so it works with AI mechs
        tgtObject = GameObject.FindGameObjectWithTag("Player");
        target = tgtObject.transform;
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
                hasSeenPlayer = true; // already handled in chase and enemy shooting code
            }

            RaycastHit hit;
            Vector3 targetPt = target.position + Vector3.up * 18.0f; // projecting off feet
            Vector3 direction = targetPt - transform.position; // think this as a 3D compass that points to this AI's target
            float angle = Vector3.Angle(direction, transform.forward); // angle is important to test whether mech is in field of view

            // next line check if vision ray hit a valid target
            bool didRayHitValidTarget = Physics.Raycast(transform.position, direction, out hit, _mech.range) && (hit.collider.gameObject.tag == "Player" || hit.collider.gameObject.tag == "Enemy");

            bool visionConeSeesTarget = false;
            bool nearAwarenessNoticesTarget = false;

            if (didRayHitValidTarget)
            {
                Debug.DrawLine(transform.position, hit.point, Color.cyan);
                visionConeSeesTarget = Vector3.Distance(targetPt, transform.position) < visionRange && angle < fieldOfViewAngle; // test if mech is in vision cone
                //Debug.Log(Mathf.RoundToInt(Vector3.Distance(targetPt, transform.position)) + " " + Mathf.RoundToInt(angle));
                nearAwarenessNoticesTarget = Vector3.Distance(targetPt, transform.position) < awarenessRange; // test if mech is too close to this AI
                //Debug.Log(visionConeSeesPlayer + " " + nearAwarenessNoticesPlayer);
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position + direction.normalized * visionRange, Color.red); // debug if ray for vision did not pass test
            }

            if (visionConeSeesTarget || nearAwarenessNoticesTarget)
            {
                tgtLastPosition = targetPt;
                agent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
                hasBeenShot = false; // may not be needed

                direction.y = 0; // removing y component to prevent mech tilt; might use later for individual mech parts
                transform.parent.transform.rotation = Quaternion.Slerp(transform.parent.transform.rotation,
                                            Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime); // smooth look at rotation

                if (Vector3.Distance(targetPt, transform.position) > attackRange) // test to see if mech is out of range 
                {

                    agent.SetDestination(target.position); // attempt to get within range
                    tgtLastPosition = hit.point; // last known position of targeted mech
                    // Debug.Log("Last player position" + playerLastPosition);
                    hasSeenPlayer = true;
                }

                else
                {
                    agent.SetDestination(enemyTransform.position); // stop moving since we're within range
                }
            }
            else
            {
                if (hasSeenPlayer == true) // think MGS AI reaction to snake; this stuff is specific to AI target's last know position
                {
                    //Debug.Log("PLAYER SEEN GO TO PLAYER");
                    // awarenessRange = 5;
                    float distanceToTarget = Vector3.Distance(tgtLastPosition, enemyTransform.transform.position);
                    float distanceThreshold = .6f;
                    if (distanceToTarget > distanceThreshold) // is last known pos out of our range?
                    {
                        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                        //Debug.Log("DistToTarget " + distanceToTarget);
                        //Debug.Log("DistToThresh " + distanceThreshold);
                        Debug.Log("Searching for player at last position");
                        agent.SetDestination(tgtLastPosition);
                    }
                    else if (distanceToTarget <= distanceThreshold || agent.velocity == Vector3.zero) // are within range of last known post
                    {
                        // Debug.Log("At last player position");
                        agent.SetDestination(enemyTransform.position);
                        agent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
                        hasSeenPlayer = false;
                    }
                }
            }
        }
        else
        {
            this.GetComponent<CapsuleCollider>().enabled = false; // basically turn off collider, AI vision, or anything that would engage this AI
        }
    }
}

