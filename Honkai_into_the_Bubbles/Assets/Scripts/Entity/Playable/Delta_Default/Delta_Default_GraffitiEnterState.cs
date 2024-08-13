using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Default_GraffitiEnterState : Playable_State
{
    private Delta_Default player;
    public Delta_Default_GraffitiEnterState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Default _player) : base(_player, _stateMachine, _animBoolName)
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
        if (finishTriggerCalled)
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