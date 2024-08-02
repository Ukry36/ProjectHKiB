using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Default_GraffitiExitState : Delta_Default_State
{
    protected int[] graffitirResult;
    public Delta_Default_GraffitiExitState(Delta_Default _player, Delta_Default_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        graffitirResult = player.GS.EndGraffiti();
    }

    public override void Update()
    {
        base.Update();
        if (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            player.savedInput = player.moveInput;
            player.SetAnimDir(player.moveInput);
            player.SkillManage(graffitirResult);
            player.StartCoroutine(player.GraffitiCooltime());
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}