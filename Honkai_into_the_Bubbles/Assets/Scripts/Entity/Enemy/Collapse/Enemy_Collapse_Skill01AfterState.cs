using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Collapse_Skill01After : Enemy_Collapse_State
{
    public Vector2 targetPos;
    public Enemy_Collapse_Skill01After(Enemy_Collapse _enemy, Enemy_Collapse_StateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            enemy.StartCoroutine(enemy.SkillCooltime(enemy.SkillArray[0]));
            stateMachine.ChangeState(enemy.AggroMoveState);
        }
    }

    public override void Exit()
    {
        enemy.MovePoint.transform.position = targetPos;
        enemy.boxCollider.enabled = true;
        base.Exit();
    }
}