using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_L_BasicAttackExitState : Playable_State
{
    public int combo = 0;
    public bool attackReserved = false;
    private Delta_L player;
    public Delta_L_BasicAttackExitState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_L _player) : base(_player, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (InputManager.instance.AttackInput || attackReserved)
        {
            player.AttackState.combo = combo + 1;
            player.AttackState.combo %= player.AttackArray.Length;
            stateMachine.ChangeState(player.AttackState);
        }
        else if (finishTriggerCalled)
        {
            stateMachine.ChangeState(player.IdleState);

        }
    }

    public override void Exit()
    {
        base.Exit();
        attackReserved = false;
        combo = 0;
    }
}
