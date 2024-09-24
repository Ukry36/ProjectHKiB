using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_L_Skill02SuccessState : Playable_State
{
    private Delta_L player;
    public Delta_L_Skill02SuccessState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_L _player) : base(_player, _stateMachine, _animBoolName)
    {
        this.player = _player;
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
        if (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
        {
            player.theStat.invincible = false;
            player.theStat.superArmor = false;
        }
        if (finishTriggerCalled)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.theStat.invincible = false;
        player.theStat.superArmor = false;
    }
}