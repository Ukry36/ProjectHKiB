using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Enemy_Lightning_AggroMoveState : Enemy_State
{
    private Enemy_Lightning enemy;
    public Enemy_Lightning_AggroMoveState(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Lightning _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
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
            if (enemy.AreaDetectTarget(enemy.backstepRadius).Length <= 0)
            {
                enemy.stateMachine.ChangeState(enemy.AggroIdleState);
            }
            else
            {
                enemy.GazePoint.position = enemy.target.position;
                enemy.moveDir = -enemy.GazePointToDir4();
                enemy.SetAnimDir(-enemy.moveDir);
                if (enemy.LineDetectTarget(enemy.GazePointToDir4(), enemy.SkillArray[0].DetectRadius, 1) && !enemy.SkillArray[0].isCooltime)
                {
                    enemy.stateMachine.ChangeState(enemy.Skill01EnterState);
                }
                else if (!enemy.MovepointAdjustCheck())
                {
                    enemy.MovePoint.transform.position += enemy.moveDir;
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
