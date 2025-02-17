using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Delta_ChargeSkillState : Playable_State
{
    private bool attackReserved;
    private Delta_Delta player;
    public Delta_Delta_ChargeSkillState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Delta _player) : base(_playerBase, _stateMachine, _animBoolName)
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
        if (controlTriggerCalled)
        {
            if (player.moveInput != Vector2.zero)
            {
                player.SetAnimDir(player.moveInput);

                player.moveDir = player.moveInput;
                for (int i = 0; i < player.ChargeSkill.TrackingRadius; i++)
                {
                    if (!player.MovepointAdjustCheck())
                    {
                        player.MovePoint.transform.position += player.moveDir;
                        player.Mover.position = player.MovePoint.transform.position;
                    }
                }
            }

            controlTriggerCalled = false;
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

        player.StartBurstMode(1);
        player.BurstAttackState.combo = 0;
        player.BurstAttackExitState.combo = -1;
    }
}
