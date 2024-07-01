using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_R_DodgeExitState : Delta_R_State
{
    public Delta_R_DodgeExitState(Delta_R _player, Delta_R_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
        if (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            player.StateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
