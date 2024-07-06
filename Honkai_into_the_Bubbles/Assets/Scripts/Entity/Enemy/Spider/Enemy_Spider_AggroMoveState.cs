using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Enemy_Spider_AggroMoveState : Enemy_Spider_State
{
    private Collider2D[] colliders;
    private bool stuckCheck = true;
    public Enemy_Spider_AggroMoveState(Enemy_Spider _enemy, Enemy_Spider_StateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
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
                colliders = Physics2D.OverlapCircleAll(enemy.MovePoint.transform.position, 0.1f, enemy.NoMovepointWallLayer);
                if (colliders != null && colliders.Length > 0)
                { enemy.MovePoint.transform.position = enemy.MovePoint.prevPos; Debug.Log("stuck"); }
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
            colliders = enemy.AreaDetectTarget(enemy.endFollowRadius);
            if (colliders == null || colliders.Length <= 0)
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
                else
                {
                    if (enemy.LineDetectTarget(enemy.moveDir, enemy.SkillArray[0].DetectRadius, 1, true) != null && !enemy.SkillArray[0].isCooltime)
                    {
                        enemy.StateMachine.ChangeState(enemy.Skill01EnterState);
                    }
                    else
                    {

                        enemy.moveDir = new Vector3(enemy.PathList[1].x, enemy.PathList[1].y) - enemy.MovePoint.transform.position;
                        enemy.SetAnimDir(enemy.moveDir);

                        if (!enemy.MovepointAdjustCheck())
                        {
                            enemy.MovePoint.transform.position += enemy.moveDir;
                        }
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
