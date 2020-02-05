using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected AICharacter agent;
    public string reasonForLastStateChange;

    public abstract void Tick();
    public abstract void FixedTick();

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

    public State(AICharacter agent, string reasonForChange)
    {
        this.agent = agent;
        reasonForLastStateChange = reasonForChange;
        Debug.Log(agent.name + reasonForLastStateChange);
    }

    public virtual string StateName() { return "undefined state"; }
}
