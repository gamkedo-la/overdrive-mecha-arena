using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : MonoBehaviour
{
    private State currentState;
    private List<Health> validTargets = new List<Health>();
    private bool isChasing = false;

    public List<Health> getValidTargets { get { return validTargets; } }
    // isChasing should only be set to true by collisions 
    public bool setIsChasing { set { isChasing = false; } }

    private void Start()
    {
        SetState(new PatrolState(this));
    }

    private void Update()
    {
        currentState.Tick();
    }

    public void SetState(State state)
    {
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

    // For OnTriggerExit we could call the return to PatrolState or enter a new state called EscapeState depending on the situation
    private void OnTriggerEnter(Collider other)
    {
        Health target = other.GetComponent<Health>();

        // Ignore collisions with non-killable objects
        if (target != null)
        {
            validTargets.Add(target);

            if (!isChasing)
            {
                SetState(new ChaseState(this));
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Add each collider that has a Health Script to a list
        // List will be used in ChaseState to determine which targets are worth attacking and which one's are not
        // List could also be used to determine when running away is the smart AI choice. 
        // For example, if the 50% of the targets in the list have over 50% HP, then this AI should run away and find a new target or hide
    }
}
