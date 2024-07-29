using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Delta_DodgeEnterState : Delta_Delta_State
{
    public Delta_Delta_DodgeEnterState(Delta_Delta _player, Delta_Delta_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.theStat.invincible = true;
        player.theStat.superArmor = true;
    }

    public override void Update()
    {
        base.Update();
        if (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            if (player.keepDodge || PlayerManager.instance.forcedKeepDodge)
            {
                player.StateMachine.ChangeState(player.DodgeState);
            }
            else
            {
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
                    if (!player.PointWallCheck(player.MovePoint.transform.position - player.savedInput))
                    {
                        player.MovePoint.transform.position -= player.savedInput;
                    }
                }
                player.Mover.position = player.MovePoint.transform.position;

                player.StateMachine.ChangeState(player.DodgeExitState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.theStat.invincible = false;
        player.theStat.superArmor = false;
    }
}
