using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                else if ((colliders = enemy.AreaDetectTarget(enemy.SkillArray[1].DetectRadius, true)).Length > 0 && ((enemy.theStat.CurrentHP / enemy.theStat.maxHP) < 0.5 || !enemy.SkillArray[0].isCooltime))
                {
                    enemy.SelectFarthestTarget(colliders);
                    Debug.Log("lineDetect success");
                    List<Vector3> availablePositions = new();

                    Vector3[] offsets = { new(-1, -1), new(1, -1), new(-1, 1), new(1, 1) };

                    foreach (Vector3 offset in offsets)
                    {
                        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.target.transform.position + offset, 0.8f);

                        bool isWallLayerPresent = false;



                        foreach (Collider2D collider in colliders)
                        {
                            if (((1 << collider.gameObject.layer) & enemy.wallLayer & ~(1 << LayerMask.NameToLayer("Player")) & ~(1 << LayerMask.NameToLayer("Movepoint")) & ~(1 << LayerMask.NameToLayer("Enemy"))) != 0)
                            {
                                isWallLayerPresent = true;
                                break;
                            }
                        }

                        if (!isWallLayerPresent)
                        {
                            availablePositions.Add(enemy.target.transform.position + offset);
                        }
                    }

                    Debug.Log(availablePositions.Count);

                    if (availablePositions.Count > 0)
                    {
                        System.Random random = new();

                        Vector3 randomPos = availablePositions[random.Next(availablePositions.Count)];

                        enemy.Skill01IngState.targetPos = randomPos;
                        enemy.StateMachine.ChangeState(enemy.Skill01BeforeState);
                    }
                }
                else if ((colliders = enemy.AreaDetectTarget(enemy.SkillArray[1].DetectRadius, true)).Length > 0 && !enemy.SkillArray[1].isCooltime)
                {
                    enemy.StateMachine.ChangeState(enemy.Skill02EnterState);
                }
                else
                {
                    enemy.GazePoint.position = enemy.target.position;
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

    public override void Exit()
    {
        base.Exit();
        enemy.Mover.position = enemy.MovePoint.transform.position;
    }
}