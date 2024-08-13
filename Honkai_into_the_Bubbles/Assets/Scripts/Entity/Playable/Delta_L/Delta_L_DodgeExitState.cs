using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_L_DodgeExitState : Playable_State
{
    private Delta_L player;
    public Delta_L_DodgeExitState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_L _player) : base(_player, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        player.DodgeCooltimeManage();
    }

    public override void Update()
    {
        base.Update();
        if (finishTriggerCalled)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
