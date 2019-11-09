using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : MonoBehaviour
{
    private State currentState;
    private Collider targetCol;
    private EnemyShooting aiShooting;

    public Collider getTargetCollider { get { return targetCol; } }
    public EnemyShooting getShootingScript { get { return aiShooting; } }

    private void Start()
    {
        aiShooting = GetComponent<EnemyShooting>();
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
        // Ignore collisions with non-killable objects
        if(other.GetComponent<Health>())
        {
            targetCol = other;

            SetState(new ChaseState(this));
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
