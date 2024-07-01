using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Delta_StateMachine
{
    public Delta_Delta_State currentState { get; private set; }

    public void Initialize(Delta_Delta_State _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(Delta_Delta_State _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
