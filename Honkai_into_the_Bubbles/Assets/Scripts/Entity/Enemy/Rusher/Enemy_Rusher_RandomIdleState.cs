using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rusher_RandomIdleState : Enemy_Rusher_State
{
    private Collider2D[] colliders;
    public Enemy_Rusher_RandomIdleState(Enemy_Rusher _enemy, Enemy_Rusher_StateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {

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
                enemy.StateMachine.ChangeState(enemy.AggroMoveState);
            }
        }
        else if (stateTimer < 0)
        {
            enemy.StateMachine.ChangeState(enemy.RandomMoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
