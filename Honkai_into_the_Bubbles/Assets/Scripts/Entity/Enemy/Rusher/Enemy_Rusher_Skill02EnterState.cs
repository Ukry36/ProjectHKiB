using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rusher_Skill02EnterState : Enemy_State
{
    private float tinkerTimer = 0;

    private Enemy_Rusher enemy;
    public Enemy_Rusher_Skill02EnterState(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Rusher _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        if (enemy.SetPath() < 2)
        {
            enemy.stateMachine.ChangeState(enemy.IdleState);
        }
        enemy.Track(enemy.SkillArray[1]);
        enemy.GazePoint.position = enemy.target.position;
        enemy.moveDir = enemy.SetVectorOne(enemy.GazePoint.position - enemy.Mover.position);
        if (enemy.moveDir.x != 0)
            enemy.moveDir.y = 0;
        enemy.SetAnimDir(enemy.moveDir);
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
