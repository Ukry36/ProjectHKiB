using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Lightning_Skill02State : Enemy_State
{
    private Enemy_Lightning enemy;
    public Enemy_Lightning_Skill02State(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Lightning _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.StartCoroutine(enemy.ShootBullet02(enemy.GazePointToDir4()));
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
