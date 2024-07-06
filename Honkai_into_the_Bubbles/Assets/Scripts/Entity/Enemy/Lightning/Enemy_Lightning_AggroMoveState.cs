using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Enemy_Lightning_AggroMoveState : Enemy_Lightning_State
{
    private Collider2D[] colliders;
    private bool stuckCheck = true;
    public Enemy_Lightning_AggroMoveState(Enemy_Lightning _enemy, Enemy_Lightning_StateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (Vector3.Distance(enemy.Mover.position, enemy.MovePoint.transform.position) >= .05f)
        {
            if (stuckCheck)
            {
                //colliders = Physics2D.OverlapCircleAll(enemy.MovePoint.transform.position, 0.1f, enemy.NoMovepointWallLayer);
                //if (colliders != null && colliders.Length > 0)
                //{ enemy.MovePoint.transform.position = enemy.MovePoint.prevPos; Debug.Log("stuck"); }
            }
            stuckCheck = false;

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
            stuckCheck = true;
            colliders = enemy.AreaDetectTarget(enemy.backstepRadius);
            if (colliders == null || colliders.Length <= 0)
            {
                enemy.StateMachine.ChangeState(enemy.AggroIdleState);
            }
            else
            {
                enemy.GazePoint.position = enemy.target.position;
                enemy.moveDir = -enemy.GazePointToDir4();
                enemy.SetAnimDir(-enemy.moveDir);
                colliders = new Collider2D[] { enemy.LineDetectTarget(enemy.GazePointToDir4(), enemy.SkillArray[0].DetectRadius, 1) };
                if (colliders[0] != null && !enemy.SkillArray[0].isCooltime)
                {
                    enemy.SelectNearestTarget(colliders);
                    enemy.StateMachine.ChangeState(enemy.Skill01EnterState);
                }
                else if (!enemy.MovepointAdjustCheck())
                    enemy.MovePoint.transform.position += enemy.moveDir;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.Mover.position = enemy.MovePoint.transform.position;
    }
}
