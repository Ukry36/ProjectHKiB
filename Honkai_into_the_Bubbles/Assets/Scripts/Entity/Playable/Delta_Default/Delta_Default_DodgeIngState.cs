using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Delta_Default_DodgeIngState : Delta_Default_State
{
    public Delta_Default_DodgeIngState(Delta_Default _player, Delta_Default_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.keepDodgeTimeLimit;
        player.theStat.superArmor = true;
        MoveCoroutine();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0 || !InputManager.instance.DodgeProgressInput)
        {
            player.StateMachine.ChangeState(player.DodgeExitState);
        }
    }

    protected IEnumerator MoveCoroutine()
    {
        float speed = player.keepDodgeSpeed * PlayerManager.instance.exKeepDodgeSpeedCoeff;
        int moveCount = 0;


        while (true)
        {
            if (moveCount >= player.keepDodgeLimit + PlayerManager.instance.exDodgeLength)
                break;

            savedInput = (Vector3)moveInput;
            player.MovePoint.transform.position += savedInput;
            yield return null;

            if (MovepointAdjustCheck())
            {
                player.MovePoint.transform.position -= savedInput;
                continue;
            }

            moveCount++;

            while (Vector3.Distance(player.Mover.position, player.MovePoint.transform.position) >= .05f)
            {
                yield return null;
                player.Mover.position =
                Vector3.MoveTowards(player.Mover.position, player.MovePoint.transform.position, speed * Time.deltaTime);
            }
            player.Mover.position = player.MovePoint.transform.position;
            player.MovePoint.prevPos = player.Mover.position; // used for external movepoint control
        }

        player.StateMachine.ChangeState(player.DodgeExitState);
    }

    public override void Exit()
    {
        base.Exit();
        SetDir(savedInput);
        player.theStat.superArmor = false;
    }
}
