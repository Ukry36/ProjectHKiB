using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Lightning_StateMachine
{
    public Enemy_Lightning_State currentState { get; private set; }

    public void Initialize(Enemy_Lightning_State _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(Enemy_Lightning_State _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
