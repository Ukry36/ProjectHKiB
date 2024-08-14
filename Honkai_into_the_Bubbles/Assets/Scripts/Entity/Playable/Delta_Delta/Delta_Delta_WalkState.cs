using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Delta_Delta_WalkState : Playable_State
{
    private Delta_Delta player;
    public Delta_Delta_WalkState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Delta _player) : base(_playerBase, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        player.StationalActivateManage(true);
    }

    public override void Update()
    {
        base.Update();

        if (InputManager.instance.AttackInput)
        {
            stateMachine.ChangeState(player.AttackState);
        }
        else if (Vector3.Distance(player.Mover.position, player.MovePoint.transform.position) >= .05f)
        {
            player.Mover.position = Vector3.MoveTowards
            (
                player.Mover.position,
                player.MovePoint.transform.position,
                player.MoveSpeed * Time.deltaTime
            );
        }
        else
        {
            player.Mover.position = player.MovePoint.transform.position; // make position accurate
            player.MovePoint.prevPos = player.Mover.position; // used in external movepoint control
            if (player.moveInput == Vector2.zero)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                // save moveinput
                player.moveDir = (Vector3)player.moveInput;

                // if there is wall, exit walkin
                // else, adjust savedInput or 
                player.SetAnimDir(player.moveDir);
                if (player.MovepointAdjustCheck())
                {
                    stateMachine.ChangeState(player.IdleState);
                }
                else
                {
                    player.MovePoint.transform.position += player.moveDir;
                    player.SetAnimDir(player.moveDir);
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.StationalActivateManage(false);
        player.Mover.position = player.MovePoint.transform.position;
        player.SetAnimDir(player.moveDir);
    }
}
