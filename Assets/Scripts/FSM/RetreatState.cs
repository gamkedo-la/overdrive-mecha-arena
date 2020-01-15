using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatState : State
{
    private Mecha mech;
    private Transform enemyPos;//AI wants to get away from it's attacker
    private Health health;

    public RetreatState(AICharacter agent) : base(agent)
    {
    }
    public override string StateName()
    {
        return "retreat state";
    }

    public override void OnStateEnter()
    {
        health = agent.GetComponent<Health>();
        enemyPos = health._myAttacker;
        mech = agent._mech;

    }

    public override void Tick()
    {
        // Determine which type of mech this AI is
        // Semi-randomly select whether this AI should run away or stand and fight (mech type will play a role in this behavior)
        // Execute retreat specific functions (again, depending on the AI's mech type semi-randomly choose whether how it run away or fight; 
        // should it use it's shield, should it just run, should its special move be used)
    }

    public override void FixedTick()
    {
        
    }
}
