using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_L_Skill02BeforeState : Delta_L_State
{
    public Skill skill;
    public Delta_L_Skill02BeforeState(Delta_L _player, Delta_L_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = skill.Delay;
        player.theStat.invincible = true;
        player.theStat.superArmor = true;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.Skill02FailState);
        }
    }

    public override void Hit(Vector3 _attackOrigin)
    {
        base.Hit(_attackOrigin);
        player.GazePoint.position = _attackOrigin;
        player.SetAnimDir(player.GazePointToDir4());

        stateMachine.ChangeState(player.Skill02SuccessState);
        //패링 성공 부분
    }

    public override void Exit()
    {
        base.Exit();
        player.theStat.invincible = false;
        player.theStat.superArmor = false;
    }
}