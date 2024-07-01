using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Default_State
{
    protected Delta_Default_StateMachine stateMachine;
    protected Delta_Default player;
    protected string animBoolName;

    protected float stateTimer;

    protected bool triggerCalled;

    public Delta_Default_State(Delta_Default _player, Delta_Default_StateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.Animator.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        player.moveInput = InputManager.instance.MoveInput;
        player.moveInput = new Vector2
        (
            player.moveInput.x == 0 ? 0 : Mathf.Sign(player.moveInput.x),
            player.moveInput.y == 0 ? 0 : Mathf.Sign(player.moveInput.y)
        );

        if (InputManager.instance.SprintInput)
            player.SetSpeedSprint();
        else
            player.SetSpeedDefault();

        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        player.Animator.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
