using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Lightning_IdleState : Enemy_State
{
    private Enemy_Lightning enemy;
    public Enemy_Lightning_IdleState(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Lightning _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
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
            enemy.stateMachine.ChangeState(enemy.RandomIdleState);
        }
        else if (enemy.moveByPathFind)
        {
            enemy.stateMachine.ChangeState(enemy.PFIdleState);
        }
        else
        {
            enemy.stateMachine.ChangeState(enemy.DirIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
