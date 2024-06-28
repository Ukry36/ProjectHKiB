using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Default_IdleState : Delta_Default_State
{
    public Delta_Default_IdleState(Delta_Default _player, Delta_Default_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (InputManager.instance.MoveInput != Vector2.zero)
        {
            savedInput = (Vector3)moveInput;
            if (MovepointAdjustCheck())
            {
                SetDir(moveInput);
            }
            else
            {
                player.StateMachine.ChangeState(player.WalkState);
            }

        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
