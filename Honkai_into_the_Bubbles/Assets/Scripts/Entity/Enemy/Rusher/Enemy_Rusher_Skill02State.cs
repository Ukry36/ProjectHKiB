using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rusher_Skill02State : Enemy_State
{
    private Enemy_Rusher enemy;
    public Enemy_Rusher_Skill02State(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Rusher _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
        if (finishTriggerCalled)
        {
            enemy.StartCoroutine(enemy.Skill02Cooltime());
            enemy.stateMachine.ChangeState(enemy.AggroMoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
