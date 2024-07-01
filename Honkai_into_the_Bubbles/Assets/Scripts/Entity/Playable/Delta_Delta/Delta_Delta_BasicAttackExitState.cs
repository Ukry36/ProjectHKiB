using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Delta_BasicAttackExitState : Delta_Delta_State
{
    public int combo = 0;
    public bool attackReserved = false;
    public Delta_Delta_BasicAttackExitState(Delta_Delta _player, Delta_Delta_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (InputManager.instance.AttackInput || attackReserved)
        {
            attackReserved = false;
            player.AttackState.combo = combo + 1;
            player.AttackState.combo %= player.AttackArray.Length;
            player.StateMachine.ChangeState(player.AttackState);
        }

        if (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            player.StateMachine.ChangeState(player.IdleState);

        }
    }

    public override void Exit()
    {
        base.Exit();
        attackReserved = false;
        combo = 0;
    }
}
