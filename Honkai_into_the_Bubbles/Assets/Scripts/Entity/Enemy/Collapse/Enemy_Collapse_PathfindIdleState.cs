using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Collapse_PathfindIdleState : Enemy_Collapse_State
{
    private Collider2D[] colliders;
    public Enemy_Collapse_PathfindIdleState(Enemy_Collapse _enemy, Enemy_Collapse_StateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        enemy.strictMoveProcess++;
        StrictMoveNode SMN = enemy.StrictMoveNodes[enemy.strictMoveProcess % enemy.StrictMoveNodes.Count];
        stateTimer = SMN.waitTime <= 0 ? Random.Range(0.5f, 3f) : SMN.waitTime;

        enemy.SetAnimDir(SMN.gazeDir);
    }

    public override void Update()
    {
        base.Update();
        if (!enemy.isDetectCooltime)
        {
            colliders = enemy.AreaDetectTarget(enemy.followRadius);
            enemy.StartCoroutine(enemy.DetectCooltime());
            if (colliders != null && colliders.Length > 0)
            {
                colliders = null;
                enemy.StateMachine.ChangeState(enemy.AggroMoveState);
            }
        }
        else if (stateTimer < 0)
        {
            enemy.StateMachine.ChangeState(enemy.PFMoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
