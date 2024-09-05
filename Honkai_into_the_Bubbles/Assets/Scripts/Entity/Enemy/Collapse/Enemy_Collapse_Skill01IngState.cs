using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Collapse_Skill01Ing : Enemy_State
{
    public Vector3 targetPos;

    private Enemy_Collapse enemy;
    public Enemy_Collapse_Skill01Ing(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Collapse _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.Skill01AfterState.targetPos = targetPos;
        enemy.Mover.position = targetPos;
        enemy.MovePoint.transform.position = targetPos;

        enemy.EnemyWallBoxCollider.enabled = true;
        enemy.MovePoint.boxCollider.enabled = false;
        enemy.boxCollider.enabled = false;
    }

    public override void Update()
    {
        base.Update();
        if (finishTriggerCalled)
        {
            stateMachine.ChangeState(enemy.Skill01AfterState);
        }
    }

    public override void Exit()
    {
        enemy.EnemyWallBoxCollider.enabled = false;
        enemy.MovePoint.boxCollider.enabled = true;
        enemy.boxCollider.enabled = true;
        base.Exit();
    }
}