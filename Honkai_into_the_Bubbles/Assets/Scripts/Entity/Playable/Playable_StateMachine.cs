using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playable_StateMachine
{
    public Playable_State currentState { get; private set; }

    public void Initialize(Playable_State _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(Playable_State _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
