using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Lightning_Skill01State : Enemy_Lightning_State
{
    Vector2 dir;
    public Enemy_Lightning_Skill01State(Enemy_Lightning _player, Enemy_Lightning_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

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
        if (enemy.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            enemy.StartCoroutine(enemy.Skill01Cooltime());
            enemy.StateMachine.ChangeState(enemy.AggroMoveState);
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
