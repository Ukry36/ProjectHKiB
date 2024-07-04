using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rusher_StateMachine
{
    public Enemy_Rusher_State currentState { get; private set; }

    public void Initialize(Enemy_Rusher_State _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(Enemy_Rusher_State _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
