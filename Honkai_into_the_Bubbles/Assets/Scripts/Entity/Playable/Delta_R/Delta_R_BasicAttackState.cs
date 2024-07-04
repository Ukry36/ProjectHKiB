using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_R_BasicAttackState : Delta_R_State
{
    public int combo = 0;
    private bool attackReserved;
    public Delta_R_BasicAttackState(Delta_R _player, Delta_R_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.Animator.SetInteger("Combo", combo);
        player.SetAnimDir(player.savedInput);
        if (player.moveInput != Vector2.zero)
        {
            player.savedInput = player.moveInput;
            for (int i = 0; i < player.AttackArray[combo].TrackingRadius; i++)
            {
                if (!player.MovepointAdjustCheck())
                {
                    player.MovePoint.transform.position += player.savedInput;
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
                player.savedInput = player.moveInput;
        }
        if (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            player.AttackExitState.attackReserved = attackReserved;

            player.AttackExitState.combo = combo;
            player.StateMachine.ChangeState(player.AttackExitState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        attackReserved = false;
        combo = 0;
    }
}
