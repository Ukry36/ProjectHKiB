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
            if (player.startAtCombo3 && !player.isBurstMode)
                player.AttackState.combo = 2;

            if (player.isBurstMode)
                stateMachine.ChangeState(player.BurstAttackState);
            else
                stateMachine.ChangeState(player.AttackState);
            return;
        }

        if (InputManager.instance.ChargeInput && !player.isBurstMode && player.canCharge)
        {
            stateMachine.ChangeState(player.ChargeSkillState);
            return;
        }

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
            return;
        }


    }

    public override void Exit()
    {
        base.Exit();
        player.StationalActivateManage(false);
    }
}
