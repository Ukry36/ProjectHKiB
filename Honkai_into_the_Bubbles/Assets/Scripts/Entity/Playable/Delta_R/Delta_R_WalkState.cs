using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Delta_R_WalkState : Delta_R_State
{
    public Delta_R_WalkState(Delta_R _player, Delta_R_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (InputManager.instance.AttackInput)
        {
            player.StateMachine.ChangeState(player.AttackState);
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
                player.StateMachine.ChangeState(player.IdleState);
            }
            else
            {
                // save moveinput
                player.savedInput = (Vector3)player.moveInput;

                // if there is wall, exit walkin
                // else, adjust savedInput or 
                player.SetAnimDir(player.savedInput);
                if (player.MovepointAdjustCheck())
                {
                    player.StateMachine.ChangeState(player.IdleState);
                }
                else
                {
                    player.MovePoint.transform.position += player.savedInput;
                    player.SetAnimDir(player.savedInput);
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.Mover.position = player.MovePoint.transform.position;
        player.SetAnimDir(player.savedInput);
    }
}
