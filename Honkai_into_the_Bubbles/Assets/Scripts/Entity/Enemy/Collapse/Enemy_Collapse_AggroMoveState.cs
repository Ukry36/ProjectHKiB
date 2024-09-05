using System.Collections.Generic;
using UnityEngine;

public class Enemy_Collapse_AggroMoveState : Enemy_State
{
    private Collider2D[] colliders;

    private Enemy_Collapse enemy;
    public Enemy_Collapse_AggroMoveState(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Collapse _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

            if ((colliders = enemy.AreaDetectTarget(enemy.endFollowRadius)).Length <= 0)
            {
                enemy.stateMachine.ChangeState(enemy.IdleState);
            }
            else
            {
                enemy.SelectNearestTarget(colliders);

                if (enemy.SetPath() < 2)
                {
                    enemy.stateMachine.ChangeState(enemy.IdleState);
                }
                else if ((colliders = enemy.AreaDetectTarget(enemy.SkillArray[0].DetectRadius, true)).Length > 0 && ((enemy.canSkill01Passive && enemy.theStat.CurrentHP / enemy.theStat.maxHP < 0.5) || !enemy.SkillArray[0].isCooltime))
                {
                    if (enemy.theStat.CurrentHP / enemy.theStat.maxHP < 0.5)
                        enemy.canSkill01Passive = false;
                    enemy.SelectFarthestTarget(colliders);
                    List<Vector3> availablePositions = new();

                    Vector3[] offsets = { new(-0.5f, -0.5f), new(-0.5f, 0.5f), new(0.5f, -0.5f), new(0.5f, 0.5f) };

                    foreach (Vector3 offset in offsets)
                    {
                        Vector3 position = new Vector3(Mathf.RoundToInt(enemy.target.transform.position.x), Mathf.RoundToInt(enemy.target.transform.position.y)) + offset;
                        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.8f);

                        bool isWallLayerPresent = false;

                        foreach (Collider2D collider in colliders)
                        {
                            if (((1 << collider.gameObject.layer) & enemy.wallLayer & ~(1 << LayerMask.NameToLayer("Player")) & ~(1 << LayerMask.NameToLayer("Movepoint"))) != 0)
                            {
                                isWallLayerPresent = true;
                                break;
                            }
                        }

                        if (!isWallLayerPresent)
                        {
                            availablePositions.Add(position);
                        }
                    }

                    if (availablePositions.Count > 0)
                    {
                        System.Random random = new();

                        Vector3 randomPos = availablePositions[random.Next(availablePositions.Count)];

                        enemy.Skill01BeforeState.targetPos = randomPos;
                        enemy.Skill01IngState.targetPos = randomPos;
                        enemy.stateMachine.ChangeState(enemy.Skill01BeforeState);
                    }
                }
                else if (enemy.LineDetectTarget(new Vector2(enemy.Animator.GetFloat("dirX"), enemy.Animator.GetFloat("dirY")), enemy.SkillArray[1].DetectRadius, 1, true) && !enemy.SkillArray[1].isCooltime)
                {
                    enemy.stateMachine.ChangeState(enemy.Skill02EnterState);
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