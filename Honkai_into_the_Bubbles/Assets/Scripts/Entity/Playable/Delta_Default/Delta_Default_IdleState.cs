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
            player.savedInput = (Vector3)player.moveInput;
            if (player.MovepointAdjustCheck())
            {
                player.SetDir(player.moveInput);
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
