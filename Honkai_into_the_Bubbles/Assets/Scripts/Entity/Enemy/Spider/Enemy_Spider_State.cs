using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spider_State
{
    protected Enemy_Spider_StateMachine stateMachine;
    protected Enemy_Spider enemy;
    protected string animBoolName;

    protected float stateTimer;

    protected bool triggerCalled;

    public Enemy_Spider_State(Enemy_Spider _player, Enemy_Spider_StateMachine _stateMachine, string _animBoolName)
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
