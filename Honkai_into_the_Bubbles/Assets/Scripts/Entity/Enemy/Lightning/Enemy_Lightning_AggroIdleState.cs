using UnityEngine;

public class Enemy_Lightning_AggroIdleState : Enemy_State
{
    private Collider2D[] colliders;

    private Enemy_Lightning enemy;
    public Enemy_Lightning_AggroIdleState(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Lightning _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
        if (!enemy.isDetectCooltime)
        {
            enemy.StartCoroutine(enemy.DetectCooltime());

            colliders = enemy.AreaDetectTarget(enemy.endFollowRadius);
            if (colliders == null || colliders.Length <= 0)
            {
                enemy.stateMachine.ChangeState(enemy.IdleState);
            }
            else
            {
                enemy.SelectNearestTarget(colliders);
                colliders = enemy.AreaDetectTarget(enemy.backstepRadius);
                if (colliders != null && colliders.Length > 0)
                {
                    enemy.stateMachine.ChangeState(enemy.AggroMoveState);
                }
                else
                {
                    colliders = new Collider2D[] { enemy.LineDetectTarget(enemy.GazePointToDir4(), enemy.SkillArray[0].DetectRadius, 1) };
                    if (!enemy.SkillArray[0].isCooltime && colliders[0] != null)
                    {
                        enemy.SelectNearestTarget(colliders);
                        enemy.stateMachine.ChangeState(enemy.Skill01EnterState);
                    }
                    else
                    {
                        colliders = enemy.AreaDetectTarget(enemy.SkillArray[1].DetectRadius);
                        if (!enemy.SkillArray[1].isCooltime && colliders != null && colliders.Length > 0)
                        {
                            enemy.SelectFarthestTarget(colliders);
                            enemy.stateMachine.ChangeState(enemy.Skill02EnterState);
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
                    enemy.stateMachine.ChangeState(enemy.IdleState);

            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
