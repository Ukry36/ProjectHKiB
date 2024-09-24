using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Delta_Delta_DodgeIngState : Playable_State
{
    private float speed;
    private int moveCount;
    private Delta_Delta player;
    public Delta_Delta_DodgeIngState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Delta _player) : base(_playerBase, _stateMachine, _animBoolName)
    {
        this.player = _player;
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
        || moveCount >= player.keepDodgeLimit + PlayerManager.instance.exKeepDodgeLength
        || !InputManager.instance.DodgeProgressInput)
        {
            stateMachine.ChangeState(player.DodgeExitState);
        }
        else if (Vector3.Distance(player.Mover.position, player.MovePoint.transform.position) >= .05f)
        {
            player.Mover.position = Vector3.MoveTowards
            (
                player.Mover.position,
                player.MovePoint.transform.position,
                speed * Time.deltaTime
            );
        }
        else
        {
            player.Mover.position = player.MovePoint.transform.position; // make position accurate
            player.MovePoint.prevPos = player.Mover.position; // used in external movepoint control
            if (player.moveInput != Vector2.zero)
            {
                // save moveinput
                player.moveDir = (Vector3)player.moveInput;

                if (!player.MovepointAdjustCheck())
                {
                    player.MovePoint.transform.position += player.moveDir;
                    moveCount++;
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.SetAnimDir(player.moveDir);
        player.theStat.superArmor = false;
        player.Mover.position = player.MovePoint.transform.position;
    }
}
