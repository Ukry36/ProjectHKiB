using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Delta_Delta_DodgeState : Playable_State
{
    private Delta_Delta player;
    public Delta_Delta_DodgeState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Delta _player) : base(_playerBase, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        player.theStat.invincible = true;
        player.theStat.superArmor = true;
        player.cannotDodgeState = true;

        player.DodgeImpact();
        //player.DodgeGuageManage(10);
        player.DodgeGuageManage(100);
        player.AttackGuageManage(100);
        if (!player.isBurstMode)
            player.Animator.SetBool("Burst", false);
            
        stateTimer = player.dodgeInvincibleTime;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(player.IdleState);
    }

    public override void Hit(Vector3 _attackOrigin)
    {
        base.Hit(_attackOrigin);
        player.DodgeGuageManage(40);
    }

    public override void Exit()
    {
        base.Exit();

        int dodgeLength = player.dodgeLength + PlayerManager.instance.exDodgeLength;

        // move to where dodgeLength reaches further
        if (player.moveInput != Vector2.zero)
        {
            Vector2 apv = dodgeLength * player.moveInput;
            for (int i = 0; i < dodgeLength; i++)
            {
                if (!player.PointWallCheck(player.MovePoint.transform.position +
                    new Vector3(apv.x - i * Mathf.Sign(apv.x), apv.y - i * Mathf.Sign(apv.y), 0f)))
                {
                    player.MovePoint.transform.position +=
                    new Vector3(apv.x - i * Mathf.Sign(apv.x), apv.y - i * Mathf.Sign(apv.y), 0f);
                    break;
                }
            }
        }
        else
        {
            if (!player.PointWallCheck(player.MovePoint.transform.position - player.moveDir))
            {
                player.MovePoint.transform.position -= player.moveDir;
            }
        }
        player.Mover.position = player.MovePoint.transform.position;

        player.DodgeImpact(1);
        player.theStat.invincible = false;
        player.theStat.superArmor = false;
        player.cannotDodgeState = false;
        player.DodgeCooltimeManage();
        if (player.startAtCombo3) player.StartAtCombo3Manage();
    }
}
