using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rusher_IdleState : Enemy_Rusher_State
{
    public Enemy_Rusher_IdleState(Enemy_Rusher _enemy, Enemy_Rusher_StateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.randomMove)
        {
            enemy.StateMachine.ChangeState(enemy.RandomIdleState);
        }
        else if (enemy.moveByPathFind)
        {
            enemy.StateMachine.ChangeState(enemy.PFIdleState);
        }
        else
        {
            enemy.StateMachine.ChangeState(enemy.DirIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
