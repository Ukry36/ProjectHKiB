using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Enemy_Collapse_RandomMoveState : Enemy_Collapse_State
{
    private int movementMultiplyer = 1;
    public Enemy_Collapse_RandomMoveState(Enemy_Collapse _enemy, Enemy_Collapse_StateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetMoveDirRandom4();
        enemy.SetAnimDir(enemy.moveDir);
        movementMultiplyer = (int)Random.Range(1f, 4.5f);
    }

    public override void Update()
    {
        base.Update();
        if (Vector3.Distance(enemy.Mover.position, enemy.MovePoint.transform.position) >= .05f)
        {
            enemy.Mover.position = Vector3.MoveTowards
            (
                enemy.Mover.position,
                enemy.MovePoint.transform.position,
                enemy.MoveSpeed * Time.deltaTime
            );
        }
        else
        {
            enemy.Mover.position = enemy.MovePoint.transform.position; // make position accurate
            enemy.MovePoint.prevPos = enemy.Mover.position; // used in external movepoint control

            if (enemy.AreaDetectTarget(enemy.followRadius).Length > 0)
            {
                enemy.StateMachine.ChangeState(enemy.AggroMoveState);
            }
            else if (movementMultiplyer < 0 || enemy.MovepointAdjustCheckFor2x2())
            {
                enemy.StateMachine.ChangeState(enemy.IdleState);
            }
            else
            {
                enemy.MovePoint.transform.position += enemy.moveDir;
                enemy.SetAnimDir(enemy.moveDir);
            }

            movementMultiplyer--;
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.Mover.position = enemy.MovePoint.transform.position;
    }
}
