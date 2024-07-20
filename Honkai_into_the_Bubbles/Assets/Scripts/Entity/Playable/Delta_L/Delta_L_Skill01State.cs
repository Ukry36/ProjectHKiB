using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_L_Skill01State : Delta_L_State
{
    public Delta_L_Skill01State(Delta_L _player, Delta_L_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.theStat.invincible = true;
        player.theStat.superArmor = true;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Hit()
    {
        base.Hit();

        //패링 성공 부분
    }

    public override void Exit()
    {
        base.Exit();

        player.theStat.invincible = false;
        player.theStat.superArmor = false;
    }
}