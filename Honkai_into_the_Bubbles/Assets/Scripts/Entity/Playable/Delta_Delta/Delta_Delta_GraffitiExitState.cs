using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Delta_GraffitiExitState : Playable_State
{
    private int[] graffitirResult;
    private Delta_Delta player;
    public Delta_Delta_GraffitiExitState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Delta _player) : base(_playerBase, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        graffitirResult = player.GS.EndGraffiti();
        player.cannotDodgeState = true;
        player.cannotGraffitiState = true;
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