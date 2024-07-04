using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rusher_Skill02State : Enemy_Rusher_State
{
    public Enemy_Rusher_Skill02State(Enemy_Rusher _player, Enemy_Rusher_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
            enemy.StartCoroutine(enemy.Skill02Cooltime());
            enemy.StateMachine.ChangeState(enemy.AggroMoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
