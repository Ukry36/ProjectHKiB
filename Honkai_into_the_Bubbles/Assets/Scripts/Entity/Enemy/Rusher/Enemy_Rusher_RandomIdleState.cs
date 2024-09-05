using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rusher_RandomIdleState : Enemy_State
{
    private Collider2D[] colliders;

    private Enemy_Rusher enemy;
    public Enemy_Rusher_RandomIdleState(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Rusher _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.strictMoveProcess++;
        stateTimer = Random.Range(0.5f, 3f);
        enemy.SetMoveDirRandom8();
        enemy.SetAnimDir(enemy.moveDir);
    }

    public override void Update()
    {
        base.Update();
        if (!enemy.isDetectCooltime)
        {
            colliders = enemy.AreaDetectTarget(enemy.followRadius);
            enemy.StartCoroutine(enemy.DetectCooltime());
            if (colliders != null && colliders.Length > 0)
            {
                colliders = null;
                enemy.stateMachine.ChangeState(enemy.AggroMoveState);
            }
        }
        else if (stateTimer < 0)
        {
            enemy.stateMachine.ChangeState(enemy.RandomMoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
