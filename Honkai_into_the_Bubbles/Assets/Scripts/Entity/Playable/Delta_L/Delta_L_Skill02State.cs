using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_L_Skill02State : Delta_L_State
{
    public Delta_L_Skill02State(Delta_L _player, Delta_L_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.theStat.superArmor = true;
        player.boxCollider.enabled = true;
        stateTimer = 3.0f;
    }

    public override void Update()
    {
        base.Update();

        if (InputManager.instance.AttackInput && stateTimer > 0)
        {
            stateMachine.ChangeState(player.AttackState);
        }

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(player.AttackState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.boxCollider.enabled = false;
        player.theStat.superArmor = false;
    }
}