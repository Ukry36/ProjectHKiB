using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Enemy_Rusher_AggroMoveState : Enemy_Rusher_State
{
    private Collider2D[] colliders;
    public Enemy_Rusher_AggroMoveState(Enemy_Rusher _enemy, Enemy_Rusher_StateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
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

            if ((colliders = enemy.AreaDetectTarget(enemy.endFollowRadius)).Length <= 0)
            {
                enemy.StateMachine.ChangeState(enemy.IdleState);
            }
            else
            {
                enemy.SelectNearestTarget(colliders);
                if (enemy.SetPath() < 2)
                {
                    enemy.StateMachine.ChangeState(enemy.IdleState);
                }
                else if (enemy.LineDetectTarget(enemy.GazePointToDir4(), enemy.SkillArray[0].DetectRadius, 1, true) && !enemy.SkillArray[0].isCooltime)
                {
                    enemy.StateMachine.ChangeState(enemy.Skill01EnterState);
                }
                else if ((colliders = enemy.AreaDetectTarget(enemy.SkillArray[1].DetectRadius, true)).Length > 0 && !enemy.SkillArray[1].isCooltime)
                {
                    enemy.SelectNearestTarget(colliders);
                    enemy.StateMachine.ChangeState(enemy.Skill02EnterState);
                }
                else
                {
                    enemy.GazePoint.position = enemy.target.position;
                    enemy.SetAnimDir(enemy.GazePointToDir4());
                    enemy.moveDir = new Vector3(enemy.PathList[1].x, enemy.PathList[1].y) - enemy.MovePoint.transform.position;
                    if (enemy.backStep)
                    {
                        enemy.moveDir *= -1;
                    }
                    if (!enemy.MovepointAdjustCheck())
                    {
                        enemy.MovePoint.transform.position += enemy.moveDir;
                    }
                }

            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.Mover.position = enemy.MovePoint.transform.position;
    }
}
