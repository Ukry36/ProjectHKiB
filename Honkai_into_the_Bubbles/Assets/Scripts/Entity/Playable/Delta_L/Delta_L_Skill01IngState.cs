using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_L_Skill01IngState : Delta_L_State
{
    public Delta_L_Skill01IngState(Delta_L _player, Delta_L_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.boxCollider.enabled = false;
    }

    public override void Update()
    {
        base.Update();

        if (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f || InputManager.instance.AttackInput)
        {
            stateMachine.ChangeState(player.Skill01AfterState);
        }
    }


    public override void Exit()
    {
        base.Exit();

        player.boxCollider.enabled = true;
    }
}