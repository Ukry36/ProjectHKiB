using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playable_State
{
    protected Playable_StateMachine stateMachine;
    protected Playable playerBase;
    protected string animBoolName;

    public float stateTimer;
    public float unscaledStateTimer;

    protected bool finishTriggerCalled;

    public Playable_State(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName)
    {
        this.playerBase = _playerBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        playerBase.Animator.SetBool(animBoolName, true);
        finishTriggerCalled = false;
    }

    public virtual void Update()
    {
        playerBase.moveInput = playerBase.SetVectorOne(InputManager.instance.MoveInput);

        if (InputManager.instance.SprintInput)
            playerBase.SetSpeedSprint();
        else
            playerBase.SetSpeedDefault();

        stateTimer -= Time.deltaTime;
        unscaledStateTimer -= Time.unscaledDeltaTime;
    }

    public virtual void Exit()
    {
        playerBase.Animator.SetBool(animBoolName, false);
    }

    public virtual void Hit(Vector3 _attackOrigin) { }

    public virtual void AnimationFinishTrigger()
    {
        finishTriggerCalled = true;
    }
}
