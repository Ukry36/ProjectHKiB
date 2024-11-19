using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Default_GraffitiExitState : Playable_State
{
    protected int[] graffitirResult;
    private Delta_Default player;
    public Delta_Default_GraffitiExitState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Default _player) : base(_player, _stateMachine, _animBoolName)
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
            player.SetAnimDir(player.moveInput);
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