using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Delta_Delta_BurstAttackState : Playable_State
{
    public int combo = 0;
    private bool attackReserved;
    private Delta_Delta player;
    public Delta_Delta_BurstAttackState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Delta _player) : base(_playerBase, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();

        player.Animator.SetInteger("Combo", combo);
        player.SetAnimDir(player.moveDir);
        if (player.moveInput != Vector2.zero)
        {
            player.moveDir = player.moveInput;
            for (int i = 0; i < player.BurstAttackArray[combo].TrackingRadius; i++)
            {
                if (!player.MovepointAdjustCheck())
                {
                    player.MovePoint.transform.position += player.moveDir;
                    player.Mover.position = player.MovePoint.transform.position;
                }
            }
        }
    }

    public override void Update()
    {
        base.Update();
        if (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f)
        {
            if (InputManager.instance.AttackInput)
                attackReserved = true;

            if (player.moveInput != Vector2.zero)
                player.moveDir = player.moveInput;
        }
        if (finishTriggerCalled)
        {
            player.BurstAttackExitState.attackReserved = attackReserved;

            player.BurstAttackExitState.combo = combo;
            stateMachine.ChangeState(player.BurstAttackExitState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (!player.isBurstMode && combo == player.BurstAttackArray.Length - 1)
            player.BurstAttackExitState.exitAnimBurst = true;

        attackReserved = false;
        combo = 0;
    }
}
