using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Delta_L_Skill02SuccessState : Playable_State
{
    public Skill skill;
    private Delta_L player;
    public Delta_L_Skill02SuccessState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_L _player) : base(_player, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        player.theStat.invincible = true;
        player.theStat.superArmor = true;
        player.ParryAnim();
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
            player.GraffitiHPHeal(1);
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
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.theStat.invincible = false;
        player.theStat.superArmor = false;
    }
}