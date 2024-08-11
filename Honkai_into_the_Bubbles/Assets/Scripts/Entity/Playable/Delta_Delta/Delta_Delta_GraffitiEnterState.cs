using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Delta_GraffitiEnterState : Playable_State
{
    private Delta_Delta player;
    public Delta_Delta_GraffitiEnterState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Delta _player) : base(_playerBase, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        player.theStat.invincible = true;
        player.theStat.superArmor = true;
        player.GS.StartGraffiti();
        player.theStat.GPControl(-1, _silence: true);
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

    }
}