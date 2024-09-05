using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Lightning_Skill01State : Enemy_State
{
    Vector2 dir;

    private Enemy_Lightning enemy;
    public Enemy_Lightning_Skill01State(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Lightning _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }


    public override void Enter()
    {
        base.Enter();
        dir = enemy.GazePointToDir4();
        enemy.SetAnimDir(dir);

        stateTimer = 0.3f;
        enemy.ShootBullet01(dir);
    }

    public override void Update()
    {
        base.Update();
        if (finishTriggerCalled)
        {
            enemy.StartCoroutine(enemy.Skill01Cooltime());
            enemy.stateMachine.ChangeState(enemy.AggroMoveState);
        }
        if (stateTimer < 0)
        {
            enemy.ShootBullet01(dir);
            stateTimer = 30f;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
