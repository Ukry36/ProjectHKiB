using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Delta_DodgeExitState : Playable_State
{
    private Delta_Delta player;
    public Delta_Delta_DodgeExitState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Delta _player) : base(_playerBase, _stateMachine, _animBoolName)
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
        if (player.startAtCombo3 && InputManager.instance.AttackInput)
        {
            player.AttackState.combo = 2;
            stateMachine.ChangeState(player.AttackState);
        }
        else if (finishTriggerCalled)
        {
            player.startAtCombo3 = false;
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
