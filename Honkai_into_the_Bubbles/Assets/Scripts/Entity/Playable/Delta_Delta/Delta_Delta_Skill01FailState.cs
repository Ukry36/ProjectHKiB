using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Delta_Skill01FailState : Playable_State
{
    public Skill skill;
    private int origDMG;
    private Delta_Delta player;
    public Delta_Delta_Skill01FailState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Delta _player) : base(_player, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        origDMG = skill.DamageCoefficient;
        skill.DamageCoefficient /= 10;
        player.theStat.invincible = true;
        player.theStat.superArmor = true;
    }

    public override void Update()
    {
        base.Update();
        if (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
        {
            player.theStat.invincible = false;
            player.theStat.superArmor = false;
        }
        if (controlTriggerCalled)
        {
            if (player.moveInput != Vector2.zero)
            {
                player.SetAnimDir(player.moveInput);

                player.moveDir = player.moveInput;
                for (int i = 0; i < skill.TrackingRadius; i++)
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
            stateMachine.ChangeState(player.BurstAttackExitState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.BurstAttackState.combo = 0;
        player.BurstAttackExitState.combo = -1;
        if (!player.isBurstMode)
        {
            player.BurstAttackExitState.exitAnimBurst = true;
            player.Animator.SetBool("Burst", true);
        }

        skill.DamageCoefficient = origDMG;
        player.theStat.invincible = false;
        player.theStat.superArmor = false;
    }
}