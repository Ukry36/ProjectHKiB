using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Default_DodgeEnterState : Delta_Default_State
{
    public Delta_Default_DodgeEnterState(Delta_Default _player, Delta_Default_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

                if (moveInput != Vector2.zero)
                    savedInput = (Vector3)moveInput;
                SetDir(savedInput);

                // move to where dodgeLength reaches further (if no moveInput, dodge backward) 
                Vector2 apv = savedInput * dodgeLength;

                for (int i = 0; i < dodgeLength; i++)
                {
                    if (!Physics2D.OverlapCircle(player.MovePoint.transform.position +
                        new Vector3(apv.x - i, apv.y - i, 0f), .4f, player.wallLayer))
                    {
                        player.MovePoint.transform.position += new Vector3(apv.x - i, apv.y - i, 0f)
                            * (moveInput == Vector2.zero ? -1 : 1);
                        break;
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
