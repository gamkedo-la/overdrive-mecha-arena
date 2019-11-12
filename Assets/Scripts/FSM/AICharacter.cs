using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : MonoBehaviour
{
    private State currentState;
    private List<Health> validTargets = new List<Health>();
    private bool isChasing = false;

    public List<Health> getValidTargets { get { return validTargets; } }

    public bool resetIsChasing { set { isChasing = false; } }

    private void Start()
    {
        SetState(new PatrolState(this));
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.Tick();
        }
    }

    public void SetState(State state)
    {
        Debug.Log(state);
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
            //Debug.Log(target.name);
            validTargets.Add(target);

            if (currentState == null || currentState.StateName() != "chase state")
            {
                SetState(new ChaseState(this));
            }
        }
    }
}
