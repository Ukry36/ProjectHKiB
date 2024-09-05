using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_State
{
    protected Enemy_StateMachine stateMachine;
    protected Enemy enemyBase;
    protected string animBoolName;

    public float stateTimer;
    public float unscaledStateTimer;

    protected bool finishTriggerCalled;


    public Enemy_State(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        enemyBase.Animator.SetBool(animBoolName, true);
        enemyBase.isTurnCooltime = false;
        finishTriggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        unscaledStateTimer -= Time.unscaledDeltaTime;
    }

    public virtual void Exit()
    {
        enemyBase.Animator.SetBool(animBoolName, false);
    }

    public virtual void Hit(Vector3 _attackOrigin) { }

    public virtual void AnimationFinishTrigger()
    {
        finishTriggerCalled = true;
    }
}
