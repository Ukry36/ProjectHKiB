using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Delta_Delta_BurstAttackExitState : Playable_State
{
    public int combo = 0;
    public bool attackReserved = false;
    public bool exitAnimBurst = false;
    private Delta_Delta player;
    public Delta_Delta_BurstAttackExitState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Delta _player) : base(_playerBase, _stateMachine, _animBoolName)
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
            player.BurstAttackState.combo = combo + 1;
            player.BurstAttackState.combo %= player.BurstAttackArray.Length;
            if (!exitAnimBurst)
                stateMachine.ChangeState(player.BurstAttackState);
        }

        if (finishTriggerCalled)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Exit()
    {
        if (exitAnimBurst || (!player.isBurstMode && !attackReserved))
        {
            player.Animator.SetBool("Burst", false);
            player.UpdateGuage();
        }
        base.Exit();
        attackReserved = false;
        exitAnimBurst = false;
        combo = 0;
    }
}
