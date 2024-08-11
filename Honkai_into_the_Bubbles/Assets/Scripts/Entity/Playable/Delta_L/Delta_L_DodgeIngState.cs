using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Delta_L_DodgeIngState : Playable_State
{
    private float speed;
    private int moveCount;
    private Delta_L player;
    public Delta_L_DodgeIngState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_L _player) : base(_player, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        player.cannotDodge = true;
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
        player.cannotDodge = false;
        player.SetAnimDir(player.savedInput);
        player.theStat.superArmor = false;
        player.Mover.position = player.MovePoint.transform.position;
    }
}
