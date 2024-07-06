using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spider_StateMachine
{
    public Enemy_Spider_State currentState { get; private set; }

    public void Initialize(Enemy_Spider_State _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(Enemy_Spider_State _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
