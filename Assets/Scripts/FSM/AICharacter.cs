 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacter : MonoBehaviour
{
    [SerializeField] private Mecha mech;

    public Transform playerTransform;
    public Transform enemy;

    public Vector3 playerLastPosition;
    public Vector3 personalLastSighting;

    public GameObject playerObject;

    public int awarenessRange = 2;

    public bool hasSeenPlayer = false;
    public bool hasBeenShot = false;

    public float turnSpeed = 0.1f;
    public float attackRange;

    public bool isAttacking;
    public bool isDying;
    public float fieldOfViewAngle = 110f;

    private SphereCollider col;
    public NavMeshAgent agent;

    private State currentState;
    private Animator animator;
    private List<Health> validTargets = new List<Health>();

    public List<Health> getValidTargets { get { return validTargets; } }

    public Animator getAnimator { get { return animator; } }
    public Mecha _mech { get { return mech; } }

    private void Start()
    {
        playerObject = GameObject.Find("Player");
        playerTransform = playerTransform = playerObject.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        isDying = GetComponent<enemyScript>().enemyIsDying;

        SetState(new PatrolState(this));
    }

    private void Update()
    {
        //Debug.Log(gameObject.name + " has this many targets: " + validTargets.Count);

        if (currentState != null)
        {
            currentState.Tick();
        }

    }

    private void FixedUpdate()
    {
        //Debug.Log(gameObject.name + " has this many targets: " + validTargets.Count);

        if (currentState != null)
        {
            currentState.FixedTick();
        }
    }

    public void activateAttack()
    {
        isAttacking = true;
    }

    public void deactivateAttack()
    {
        isAttacking = false;
    }

    public void SetState(State state)
    {
        //Debug.Log(state);
        if (currentState != null)
        {
            currentState.OnStateExit();
        }

        currentState = state;

        if (currentState != null)
        {
            currentState.OnStateEnter();
        }
    }


    public void RemoveTargetFromSuperList(Health tgt)
    {
        validTargets.Remove(tgt);
    }

    // For OnTriggerExit we could call the return to PatrolState or enter a new state called EscapeState depending on the situation
    private void OnTriggerStay(Collider other)
    {
        Health target = other.GetComponent<Health>();

        // Ignore collisions with non-killable objects
        if (target != null && validTargets.Contains(target) == false)
        {
            //Debug.Log(target.name + " entered " + gameObject.name + " detection radius");

            validTargets.Add(target);

            if (currentState == null || currentState.StateName() != "chase state")
            {
                SetState(new ChaseState(this));
            }
        }
    }
} 
