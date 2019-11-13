using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatState : State
{

    public RetreatState(AICharacter agent) : base(agent)
    {

    }
    public override string StateName()
    {
        return "retreat state";
    }

    public override void OnStateEnter()
    {
        
    }

    public override void Tick()
    {
        // Execute retreat specific functions
    }
}
