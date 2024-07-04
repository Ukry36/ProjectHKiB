using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_L_StateMachine
{
    public Delta_L_State currentState { get; private set; }

    public void Initialize(Delta_L_State _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(Delta_L_State _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
