using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_L_GraffitiExitState : Playable_State
{
    private int[] graffitirResult;
    private Delta_L player;
    public Delta_L_GraffitiExitState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_L _player) : base(_player, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        player.cannotDodgeState = true;
        player.cannotGraffitiState = true;
        graffitirResult = player.GS.EndGraffiti();
        if (graffitirResult[0] == 1)
        {
            player.moveDir = player.moveInput;
            player.SetAnimDir(player.moveDir);
            player.SkillManage(graffitirResult);
            player.StartCoroutine(player.GraffitiCooltime());
        }
    }

    public override void Update()
    {
        base.Update();
        if (finishTriggerCalled)
        {
            player.moveDir = player.moveInput;
            player.SetAnimDir(player.moveDir);
            player.SkillManage(graffitirResult);
            player.StartCoroutine(player.GraffitiCooltime());
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.cannotDodgeState = false;
        player.cannotGraffitiState = false;
    }
}