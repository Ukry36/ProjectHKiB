using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_L_GraffitiEnterState : Delta_L_State
{
    public Delta_L_GraffitiEnterState(Delta_L _player, Delta_L_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.theStat.invincible = true;
        player.theStat.superArmor = true;
        player.GS.StartGraffiti();
    }

    public override void Update()
    {
        base.Update();
        if (InputManager.instance.GraffitiEndInput)
        {

            player.SkillManage(player.GS.EndGraffiti());
            player.StateMachine.ChangeState(player.IdleState);
        }
        if (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            player.StateMachine.ChangeState(player.GraffitiState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.theStat.invincible = false;
        player.theStat.superArmor = false;

    }
}