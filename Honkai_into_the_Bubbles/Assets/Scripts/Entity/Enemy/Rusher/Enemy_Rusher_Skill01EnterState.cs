using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rusher_Skill01EnterState : Enemy_Rusher_State
{
    private float tinkerTimer = 0;
    public Enemy_Rusher_Skill01EnterState(Enemy_Rusher _player, Enemy_Rusher_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        if (enemy.SetPath() < 2)
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);
        }
        enemy.Track(enemy.SkillArray[0]);
        enemy.GazePoint.position = enemy.target.position;
        enemy.moveDir = enemy.SetVectorOne(enemy.GazePoint.position - enemy.Mover.position);
        if (enemy.moveDir.x != 0)
            enemy.moveDir.y = 0;
        enemy.SetAnimDir(enemy.moveDir);
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
            enemy.StateMachine.ChangeState(enemy.Skill01State);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
