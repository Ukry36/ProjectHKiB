using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Collapse_Skill01Ing : Enemy_Collapse_State
{
    public Vector3 targetPos;

    public Enemy_Collapse_Skill01Ing(Enemy_Collapse _enemy, Enemy_Collapse_StateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {

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
        if (enemy.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
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