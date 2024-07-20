using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_R_GraffitiEnterState : Delta_R_State
{
    public Delta_R_GraffitiEnterState(Delta_R _player, Delta_R_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.theStat.invincible = true;
        player.theStat.superArmor = true;
        player.GS.StartGraffiti(); Debug.Log("sccs");
    }

    public override void Update()
    {
        base.Update();
        if (InputManager.instance.GraffitiEndInput)
        {
            player.StateMachine.ChangeState(player.GraffitiExitState);
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