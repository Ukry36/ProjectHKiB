using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Default_IdleState : Playable_State
{
    private Delta_Default player;
    public Delta_Default_IdleState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Default _player) : base(_player, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        player.StationalActivateManage(true);
    }

    public override void Update()
    {
        base.Update();
        if (InputManager.instance.MoveInput != Vector2.zero)
        {
            player.moveDir = (Vector3)player.moveInput;
            if (player.MovepointAdjustCheck())
            {
                player.SetAnimDir(player.moveInput);
            }
            else
            {
                stateMachine.ChangeState(player.WalkState);
            }

        }
    }

    public override void Exit()
    {
        base.Exit();
        player.StationalActivateManage(false);
    }
}
