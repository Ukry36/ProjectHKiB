using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spider_Skill01EnterState : Enemy_State
{
    private float tinkerTimer = 0;

    private Enemy_Spider enemy;
    public Enemy_Spider_Skill01EnterState(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Spider _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
        enemy.Track(enemy.SkillArray[0]);
        tinkerTimer = enemy.SkillArray[0].animationPlayTime - enemy.SkillArray[0].Delay;
        stateTimer = enemy.SkillArray[0].animationPlayTime;
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
            enemy.stateMachine.ChangeState(enemy.Skill01State);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
