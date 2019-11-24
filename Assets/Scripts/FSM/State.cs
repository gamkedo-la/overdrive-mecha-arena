﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected AICharacter agent;

    public abstract void Tick();
    public abstract void FixedTick();

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

    public State(AICharacter agent)
    {
        this.agent = agent;
    }

    public virtual string StateName() { return "undefined state"; }
}
