using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Delta_Skill01BeforeState : Playable_State
{
    public Skill skill;
    private bool failed = false;
    private Delta_Delta player;
    public Delta_Delta_Skill01BeforeState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Delta _player) : base(_player, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        failed = false;
        stateTimer = skill.Delay;
        player.theStat.invincible = true;
        player.theStat.superArmor = true;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            failed = true;
            player.theStat.invincible = false;
        }
        if (finishTriggerCalled)
        {
            stateMachine.ChangeState(player.Skill01FailState);
        }
    }

    public override void Hit(Vector3 _attackOrigin)
    {
        base.Hit(_attackOrigin);
        player.GazePoint.position = _attackOrigin;
        player.SetAnimDir(player.GazePointToDir4());
        stateMachine.ChangeState(failed ? player.Skill01FailState : player.Skill01SuccessState);
    }

    public override void Exit()
    {
        base.Exit();
        player.theStat.invincible = false;
        player.theStat.superArmor = false;
    }
}