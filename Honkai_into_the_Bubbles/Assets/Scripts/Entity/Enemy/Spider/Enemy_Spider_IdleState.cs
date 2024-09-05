using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spider_IdleState : Enemy_State
{
    private Enemy_Spider enemy;
    public Enemy_Spider_IdleState(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Spider _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
