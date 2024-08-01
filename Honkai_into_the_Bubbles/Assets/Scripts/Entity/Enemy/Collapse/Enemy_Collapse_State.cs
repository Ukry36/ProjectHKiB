using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Collapse_State
{
    protected Enemy_Collapse_StateMachine stateMachine;
    protected Enemy_Collapse enemy;
    protected string animBoolName;

    protected float stateTimer;

    protected bool triggerCalled;

    public Enemy_Collapse_State(Enemy_Collapse _player, Enemy_Collapse_StateMachine _stateMachine, string _animBoolName)
    {
        enemy = _player;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        enemy.Animator.SetBool(animBoolName, true);
        enemy.isTurnCooltime = false;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemy.Animator.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
