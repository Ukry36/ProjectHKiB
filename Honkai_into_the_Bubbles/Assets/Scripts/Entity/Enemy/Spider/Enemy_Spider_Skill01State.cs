using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spider_Skill01State : Enemy_State
{
    private int i = 0;

    private Enemy_Spider enemy;
    public Enemy_Spider_Skill01State(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Spider _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
        if (i < 12 && !enemy.MovepointAdjustCheck())
        {
            enemy.MovePoint.transform.position += enemy.moveDir;
            enemy.Mover.position = enemy.MovePoint.transform.position;
        }
        else
            enemy.Die();
        i++;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
