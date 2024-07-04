using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_L_IdleState : Delta_L_State
{
    public Delta_L_IdleState(Delta_L _player, Delta_L_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (InputManager.instance.AttackInput)
        {
            player.StateMachine.ChangeState(player.AttackState);
        }
        else if (InputManager.instance.MoveInput != Vector2.zero)
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
