using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_L_DodgeEnterState : Playable_State
{
    private Delta_L player;
    public Delta_L_DodgeEnterState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_L _player) : base(_player, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        player.cannotDodge = true;
        player.cannotGraffiti = true;
        player.theStat.invincible = true;
        player.theStat.superArmor = true;
    }

    public override void Update()
    {
        base.Update();
        if (finishTriggerCalled)
        {
            if (player.keepDodge || PlayerManager.instance.forcedKeepDodge)
            {
                stateMachine.ChangeState(player.DodgeState);
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

                stateMachine.ChangeState(player.DodgeExitState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.cannotDodge = false;
        player.cannotGraffiti = false;
        player.theStat.invincible = false;
        player.theStat.superArmor = false;
    }
}
