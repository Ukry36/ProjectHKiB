using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Lightning_Skill02EnterState : Enemy_State
{
    private float tinkerTimer = 0;

    private Enemy_Lightning enemy;
    public Enemy_Lightning_Skill02EnterState(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Lightning _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.GazePoint.position = enemy.target.position;
        enemy.SetAnimDir(enemy.GazePointToDir4());
        tinkerTimer = enemy.SkillArray[1].animationPlayTime - enemy.SkillArray[1].Delay;
        stateTimer = enemy.SkillArray[1].animationPlayTime;
    }

    public override void Update()
    {
        base.Update();
        tinkerTimer -= Time.deltaTime;
        if (tinkerTimer < 0)
        {
            enemy.BeforeAttackTinker(Vector3.zero);
            tinkerTimer += 20f;
        }
        if (stateTimer < 0)
        {
            enemy.stateMachine.ChangeState(enemy.Skill02State);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
