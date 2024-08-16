using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Collapse_Skill01Before : Enemy_Collapse_State
{
    public Enemy_Collapse_Skill01Before(Enemy_Collapse _enemy, Enemy_Collapse_StateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
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
            stateMachine.ChangeState(enemy.Skill01IngState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}