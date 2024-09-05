using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Collapse_Skill01After : Enemy_State
{
    public Vector2 targetPos;

    private Enemy_Collapse enemy;
    public Enemy_Collapse_Skill01After(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Collapse _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        enemy.EnemyWallBoxCollider.enabled = true;
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (finishTriggerCalled)
        {
            enemy.StartCoroutine(enemy.SkillCooltime(enemy.SkillArray[0]));
            stateMachine.ChangeState(enemy.AggroMoveState);
        }
    }

    public override void Exit()
    {
        enemy.EnemyWallBoxCollider.enabled = false;
        base.Exit();
    }
}