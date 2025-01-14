using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Delta_Skill02State : Playable_State
{
    public Skill skill;
    private bool attackReserved;
    private Delta_Delta player;
    public Delta_Delta_Skill02State(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Delta _player) : base(_playerBase, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        attackReserved = false;
        player.theStat.superArmor = true;
        player.theStat.invincible = true;
        player.cannotDodgeState = true;
        player.cannotGraffitiState = true;
    }

    public override void Update()
    {
        base.Update();
        if (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f)
        {
            if (InputManager.instance.AttackInput)
                attackReserved = true;

            if (player.moveInput != Vector2.zero)
                player.moveDir = player.moveInput;
        }
        if (finishTriggerCalled)
        {
            player.BurstAttackExitState.attackReserved = attackReserved;
            stateMachine.ChangeState(player.BurstAttackExitState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        attackReserved = false;
        player.theStat.superArmor = false;
        player.theStat.invincible = false;
        player.cannotDodgeState = false;
        player.cannotGraffitiState = false;
        if (player.isBurstMode)
            player.MaintainBurstMode(2);
        player.BurstAttackState.combo = 2;
        player.BurstAttackExitState.combo = 1;
    }
}
