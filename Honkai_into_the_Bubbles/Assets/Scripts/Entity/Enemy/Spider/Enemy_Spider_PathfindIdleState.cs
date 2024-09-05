using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spider_PathfindIdleState : Enemy_State
{
    private Collider2D[] colliders;

    private Enemy_Spider enemy;
    public Enemy_Spider_PathfindIdleState(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Spider _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.strictMoveProcess++;
        StrictMoveNode SMN = enemy.StrictMoveNodes[enemy.strictMoveProcess % enemy.StrictMoveNodes.Count];
        stateTimer = SMN.waitTime <= 0 ? Random.Range(0.5f, 3f) : SMN.waitTime;

        enemy.SetAnimDir(SMN.gazeDir);
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
            enemy.stateMachine.ChangeState(enemy.PFMoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
