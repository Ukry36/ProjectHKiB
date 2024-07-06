using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Lightning_Skill02EnterState : Enemy_Lightning_State
{
    private float tinkerTimer = 0;
    public Enemy_Lightning_Skill02EnterState(Enemy_Lightning _player, Enemy_Lightning_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

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
            enemy.StateMachine.ChangeState(enemy.Skill02State);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
