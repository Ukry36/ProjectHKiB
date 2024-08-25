using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Collapse_Skill01Before : Enemy_Collapse_State
{
    public Vector3 targetPos;
    public Enemy_Collapse_Skill01Before(Enemy_Collapse _enemy, Enemy_Collapse_StateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {

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
        if (enemy.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            stateMachine.ChangeState(enemy.Skill01IngState);
        }
    }

    public override void Exit()
    {
        enemy.EnemyWallBoxCollider.transform.localPosition = Vector3.zero;
        enemy.EnemyWallBoxCollider.enabled = false;
        base.Exit();
    }
}