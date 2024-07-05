using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rusher_State
{
    protected Enemy_Rusher_StateMachine stateMachine;
    protected Enemy_Rusher enemy;
    protected string animBoolName;

    protected float stateTimer;

    protected bool triggerCalled;

    public Enemy_Rusher_State(Enemy_Rusher _player, Enemy_Rusher_StateMachine _stateMachine, string _animBoolName)
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
