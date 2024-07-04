using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rusher_Skill01State : Enemy_Rusher_State
{
    public Enemy_Rusher_Skill01State(Enemy_Rusher _player, Enemy_Rusher_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        for (int i = 0; i < 3; i++)
            if (!enemy.MovepointAdjustCheck())
            {
                enemy.MovePoint.transform.position += enemy.moveDir;
                enemy.Mover.position = enemy.MovePoint.transform.position;
            }
            else break;

    }

    public override void Update()
    {
        base.Update();
        if (enemy.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            enemy.StartCoroutine(enemy.Skill01Cooltime());
            enemy.StateMachine.ChangeState(enemy.AggroMoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
