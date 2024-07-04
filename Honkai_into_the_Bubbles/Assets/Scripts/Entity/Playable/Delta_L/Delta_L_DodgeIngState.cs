using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Delta_L_DodgeIngState : Delta_L_State
{
    private float speed;
    private int moveCount;
    private bool stuckCheck = true;
    public Delta_L_DodgeIngState(Delta_L _player, Delta_L_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.keepDodgeTimeLimit;
        player.theStat.superArmor = true;
        speed = player.keepDodgeSpeed * PlayerManager.instance.exKeepDodgeSpeedCoeff;
        moveCount = 0;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0
        || moveCount >= player.keepDodgeLimit + PlayerManager.instance.exDodgeLength
        || !InputManager.instance.DodgeProgressInput)
        {
            player.StateMachine.ChangeState(player.DodgeExitState);
        }

        if (Vector3.Distance(player.Mover.position, player.MovePoint.transform.position) >= .05f)
        {
            if (stuckCheck)
            {
                if (Physics2D.OverlapCircle(player.MovePoint.transform.position, 0.1f, player.wallLayer))
                    player.MovePoint.transform.position = player.MovePoint.prevPos;
                stuckCheck = false; //Debug.Log("stuckCheck");
            }

            player.Mover.position = Vector3.MoveTowards
            (
                player.Mover.position,
                player.MovePoint.transform.position,
                player.keepDodgeSpeed * Time.deltaTime
            );
        }
        else
        {
            player.Mover.position = player.MovePoint.transform.position; // make position accurate
            player.MovePoint.prevPos = player.Mover.position; // used in external movepoint control
            stuckCheck = true;
            if (player.moveInput != Vector2.zero)
            {
                // save moveinput
                player.savedInput = (Vector3)player.moveInput;

                if (!player.MovepointAdjustCheck())
                {
                    player.MovePoint.transform.position += player.savedInput;
                    moveCount++;
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.SetDir(player.savedInput);
        player.theStat.superArmor = false;
    }
}
