
using UnityEngine;

public class Enemy_Spider_AggroMoveState : Enemy_State
{
    private Collider2D[] colliders;

    private Enemy_Spider enemy;
    public Enemy_Spider_AggroMoveState(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Spider _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
            colliders = enemy.AreaDetectTarget(enemy.endFollowRadius);
            if (colliders.Length <= 0)
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
                else if (enemy.LineDetectTarget(enemy.moveDir, enemy.SkillArray[0].DetectRadius, 1, true) && !enemy.SkillArray[0].isCooltime)
                {
                    enemy.stateMachine.ChangeState(enemy.Skill01EnterState);
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

    public override void Exit()
    {
        base.Exit();
        enemy.Mover.position = enemy.MovePoint.transform.position;
    }
}
