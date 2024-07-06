using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Enemy_Lightning_AggroIdleState : Enemy_Lightning_State
{
    private Collider2D[] colliders;
    public Enemy_Lightning_AggroIdleState(Enemy_Lightning _enemy, Enemy_Lightning_StateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (!enemy.isDetectCooltime)
        {
            enemy.StartCoroutine(enemy.DetectCooltime());

            colliders = enemy.AreaDetectTarget(enemy.endFollowRadius);
            if (colliders == null || colliders.Length <= 0)
            {
                enemy.StateMachine.ChangeState(enemy.IdleState);
            }
            else
            {
                enemy.SelectNearestTarget(colliders);
                colliders = enemy.AreaDetectTarget(enemy.backstepRadius);
                if (colliders != null && colliders.Length > 0)
                {
                    enemy.StateMachine.ChangeState(enemy.AggroMoveState);
                }
                else
                {
                    colliders = new Collider2D[] { enemy.LineDetectTarget(enemy.GazePointToDir4(), enemy.SkillArray[0].DetectRadius, 1) };
                    if (!enemy.SkillArray[0].isCooltime && colliders[0] != null)
                    {
                        enemy.SelectNearestTarget(colliders);
                        enemy.StateMachine.ChangeState(enemy.Skill01EnterState);
                    }
                    else
                    {
                        colliders = enemy.AreaDetectTarget(enemy.SkillArray[1].DetectRadius);
                        if (!enemy.SkillArray[1].isCooltime && colliders != null && colliders.Length > 0)
                        {
                            enemy.SelectFarthestTarget(colliders);
                            enemy.StateMachine.ChangeState(enemy.Skill02EnterState);
                        }
                    }
                }
            }
        }
        else
        {
            if (!enemy.isTurnCooltime)
            {
                colliders = enemy.AreaDetectTarget(enemy.endFollowRadius);
                if (colliders != null && colliders.Length > 0)
                {
                    enemy.SelectNearestTarget(colliders);
                    enemy.GazePoint.position = enemy.target.position;
                    enemy.SetAnimDir(enemy.GazePointToDir4());
                    enemy.StartCoroutine(enemy.TurnCooltime());
                }
                else
                    enemy.StateMachine.ChangeState(enemy.IdleState);

            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
