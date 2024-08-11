using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Delta_IdleState : Playable_State
{
    private Delta_Delta player;
    public Delta_Delta_IdleState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Delta _player) : base(_playerBase, _stateMachine, _animBoolName)
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
        if (InputManager.instance.AttackInput)
        {
            stateMachine.ChangeState(player.AttackState);
        }
        else if (InputManager.instance.MoveInput != Vector2.zero)
        {
            player.savedInput = (Vector3)player.moveInput;
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
