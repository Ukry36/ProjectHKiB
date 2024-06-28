using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Default_StateMachine
{
    public Delta_Default_State currentState { get; private set; }

    public void Initialize(Delta_Default_State _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(Delta_Default_State _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
