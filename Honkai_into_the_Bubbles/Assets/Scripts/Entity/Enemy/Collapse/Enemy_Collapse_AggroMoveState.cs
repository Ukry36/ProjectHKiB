using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Enemy_Collapse_AggroMoveState : Enemy_Collapse_State
{
    private Collider2D[] colliders;
    public Enemy_Collapse_AggroMoveState(Enemy_Collapse _enemy, Enemy_Collapse_StateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
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
            return;
        }
        enemy.Mover.position = enemy.MovePoint.transform.position; // make position accurate
        enemy.MovePoint.prevPos = enemy.Mover.position; // used in external movepoint control
        colliders = enemy.AreaDetectTarget(enemy.endFollowRadius);
        if (colliders.Length <= 0)
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);
            return;
        }
        enemy.SelectNearestTarget(colliders);
        if (enemy.SetPath() < 2)
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);
            return;
        }

        if (enemy.LineDetectTarget(enemy.moveDir, enemy.SkillArray[0].DetectRadius, 1, true))
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(enemy.target.transform.position, new Vector2(2, 2), 0);

            bool isWallLayerPresent = false;

            foreach (Collider2D collider in colliders)
            {
                if (((1 << collider.gameObject.layer) & enemy.wallLayer & ~(1 << LayerMask.NameToLayer("Player")) & ~(1 << LayerMask.NameToLayer("Movepoint")) & ~(1 << LayerMask.NameToLayer("Enemy"))) != 0)
                {
                    isWallLayerPresent = true;
                    break;
                }
            }
            if (!isWallLayerPresent && ((enemy.theStat.CurrentHP / enemy.theStat.maxHP) < 0.5 || !enemy.SkillArray[0].isCooltime))
            {
                enemy.StateMachine.ChangeState(enemy.Skill01BeforeState);
                return;
            }
        }

        if (enemy.LineDetectTarget(enemy.moveDir, enemy.SkillArray[1].DetectRadius, 1, true) && !enemy.SkillArray[1].isCooltime)
        {
            enemy.StateMachine.ChangeState(enemy.Skill02EnterState);
            return;
        }

        enemy.moveDir = new Vector3(enemy.PathList[1].x, enemy.PathList[1].y) - enemy.MovePoint.transform.position;
        enemy.SetAnimDir(enemy.moveDir);

        if (!enemy.MovepointAdjustCheck())
        {
            enemy.MovePoint.transform.position += enemy.moveDir;
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.Mover.position = enemy.MovePoint.transform.position;
    }
}