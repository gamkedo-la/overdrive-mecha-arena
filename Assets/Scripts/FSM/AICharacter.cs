using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacter : MonoBehaviour
{
    [SerializeField] private Mecha mech;
    public Transform debugPoint;

    private SphereCollider col;
    public NavMeshAgent agent;
    private EnemyShooting shootingScript;
    private Health agentHealth;

    private State currentState;
    private Animator animator;
    private List<Health> validTargets = new List<Health>();

    public List<Health> getValidTargets { get { return validTargets; } }

    public Animator getAnimator { get { return animator; } }
    public Mecha _mech { get { return mech; } }
    public State _currentState { get { return currentState; } }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        shootingScript = GetComponent<EnemyShooting>();
        agentHealth = GetComponent<Health>();

        SetState(new PatrolState(this, "initial state is patrol"));
    }

    private void Update()
    {
        //Debug.Log(gameObject.name + " has this many targets: " + validTargets.Count);

        if (currentState != null)
        {
            //Debug.Log(currentState.StateName());
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

    public void AddTgtToSuperList(Health tgt)
    {
        validTargets.Add(tgt);
    }

    public void SetChaseStateViaFieldOfView()
    {
        if (agentHealth.getCurrentHP >= agentHealth.getBaseHP / 4 && currentState.StateName() != "chase state")
        {
            if (currentState != null)
            {
                currentState.OnStateExit();
            }

            currentState = new ChaseState(this, "chasing target since it entered field of view");

            if (currentState != null)
            {
                currentState.OnStateEnter();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Health target = other.GetComponent<Health>();

            // Ignore collisions with non-killable objects
            if (target != null && validTargets.Contains(target) == false)
            {
                //Debug.Log(target.name + " entered " + gameObject.name + " detection radius");
                shootingScript._hasLostTgt = false;

                validTargets.Add(target);

                if (currentState == null || currentState.StateName() != "chase state" && agentHealth.getCurrentHP >= agentHealth.getBaseHP / 4)
                {
                    SetState(new ChaseState(this, " chasing target since it entered near awareness range"));
                }
            }
            else
            {
                shootingScript._hasLostTgt = false;

                if (currentState == null || currentState.StateName() != "chase state" && agentHealth.getCurrentHP >= agentHealth.getBaseHP / 4)
                {
                    SetState(new ChaseState(this, " chasing target since it entered near awareness range"));
                }
            }
        }
    }
}
