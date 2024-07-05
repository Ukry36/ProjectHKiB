using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Lightning_State
{
    protected Enemy_Lightning_StateMachine stateMachine;
    protected Enemy_Lightning enemy;
    protected string animBoolName;

    protected float stateTimer;

    protected bool triggerCalled;

    public Enemy_Lightning_State(Enemy_Lightning _player, Enemy_Lightning_StateMachine _stateMachine, string _animBoolName)
    {
        this.enemy = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
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
