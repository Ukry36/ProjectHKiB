using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_L_GraffitiEnterState : Playable_State
{
    private Delta_L player;
    public Delta_L_GraffitiEnterState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_L _player) : base(_player, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        player.theStat.invincible = true;
        player.theStat.superArmor = true;
        player.cannotDodgeState = true;
        player.cannotGraffitiState = true;
        player.GS.StartGraffiti();
    }

    public override void Update()
    {
        base.Update();
        if (InputManager.instance.GraffitiEndInput)
        {
            stateMachine.ChangeState(player.GraffitiExitState);
        }
        else if (finishTriggerCalled)
        {
            stateMachine.ChangeState(player.GraffitiState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.theStat.invincible = false;
        player.theStat.superArmor = false;
        player.cannotDodgeState = false;
        player.cannotGraffitiState = false;
    }
}