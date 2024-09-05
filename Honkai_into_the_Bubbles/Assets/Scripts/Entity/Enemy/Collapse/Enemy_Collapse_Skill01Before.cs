using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Collapse_Skill01Before : Enemy_State
{
    public Vector3 targetPos;

    private Enemy_Collapse enemy;
    public Enemy_Collapse_Skill01Before(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Collapse _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        enemy.EnemyWallBoxCollider.transform.position = targetPos;
        enemy.EnemyWallBoxCollider.enabled = true;
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (finishTriggerCalled)
        {
            stateMachine.ChangeState(enemy.Skill01IngState);
        }
    }

    public override void Exit()
    {
        enemy.EnemyWallBoxCollider.transform.localPosition = Vector3.zero;
        enemy.EnemyWallBoxCollider.enabled = false;
        enemy.BeforeAttackTinker(Vector3.zero);
        base.Exit();
    }
}