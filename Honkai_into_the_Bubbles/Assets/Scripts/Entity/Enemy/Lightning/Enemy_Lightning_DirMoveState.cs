using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Enemy_Lightning_DirMoveState : Enemy_Lightning_State
{
    private int movementMultiplyer = 1;
    private StrictMoveNode SMN;
    private bool stuckCheck = true;
    private Collider2D[] colliders;
    public Enemy_Lightning_DirMoveState(Enemy_Lightning _enemy, Enemy_Lightning_StateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        SMN = enemy.StrictMoveDirections[enemy.strictMoveProcess % enemy.StrictMoveDirections.Count];
        movementMultiplyer = SMN.movementMultiplyer;
    }

    public override void Update()
    {
        base.Update();
        if (Vector3.Distance(enemy.Mover.position, enemy.MovePoint.transform.position) >= .05f)
        {
            if (stuckCheck)
            {
                //colliders = Physics2D.OverlapCircleAll(enemy.MovePoint.transform.position, 0.1f, enemy.NoMovepointWallLayer);
                //if (colliders != null && colliders.Length > 0)
                //{ enemy.MovePoint.transform.position = enemy.MovePoint.prevPos; Debug.Log("stuck"); }
            }
            stuckCheck = false;

            enemy.Mover.position = Vector3.MoveTowards
            (
                enemy.Mover.position,
                enemy.MovePoint.transform.position,
                enemy.MoveSpeed * Time.deltaTime
            );
        }
        else
        {
            enemy.Mover.position = enemy.MovePoint.transform.position; // make position accurate
            enemy.MovePoint.prevPos = enemy.Mover.position; // used in external movepoint control
            stuckCheck = true;
            colliders = enemy.AreaDetectTarget(enemy.followRadius);

            if (colliders != null && colliders.Length > 0)
            {
                enemy.StateMachine.ChangeState(enemy.AggroIdleState);
            }
            else if (movementMultiplyer < 0 || enemy.MovepointAdjustCheck())
            {
                enemy.StateMachine.ChangeState(enemy.IdleState);
            }
            else
            {
                enemy.MovePoint.transform.position += enemy.moveDir;
                enemy.SetAnimDir(enemy.moveDir);
            }

            movementMultiplyer--;
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.Mover.position = enemy.MovePoint.transform.position;
    }
}
