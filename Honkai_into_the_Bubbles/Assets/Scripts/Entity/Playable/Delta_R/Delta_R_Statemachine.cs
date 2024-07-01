using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_R_StateMachine
{
    public Delta_R_State currentState { get; private set; }

    public void Initialize(Delta_R_State _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(Delta_R_State _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
