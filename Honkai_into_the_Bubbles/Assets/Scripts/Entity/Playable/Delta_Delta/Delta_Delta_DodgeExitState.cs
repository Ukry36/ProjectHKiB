using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Delta_DodgeExitState : Delta_Delta_State
{
    public Delta_Delta_DodgeExitState(Delta_Delta _player, Delta_Delta_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.DodgeCooltimeManage();
    }

    public override void Update()
    {
        base.Update();
        if (player.startAtCombo3 && InputManager.instance.AttackInput)
        {
            player.AttackState.combo = 2;
            player.StateMachine.ChangeState(player.AttackState);
        }
        else if (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            player.startAtCombo3 = false;
            player.StateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
