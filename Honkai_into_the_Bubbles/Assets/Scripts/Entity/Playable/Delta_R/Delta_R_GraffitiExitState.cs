using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_R_GraffitiExitState : Playable_State
{
    private int[] graffitirResult;
    private Delta_R player;
    public Delta_R_GraffitiExitState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_R _player) : base(_playerBase, _stateMachine, _animBoolName)
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
            if (player.moveInput != Vector2.zero)
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