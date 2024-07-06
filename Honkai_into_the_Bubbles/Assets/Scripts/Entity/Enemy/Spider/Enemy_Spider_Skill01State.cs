using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spider_Skill01State : Enemy_Spider_State
{
    private int i = 0;
    public Enemy_Spider_Skill01State(Enemy_Spider _player, Enemy_Spider_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

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
