using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Enemy_Collapse_PathfindMoveState : Enemy_Collapse_State
{
    private StrictMoveNode SMN;
    public Enemy_Collapse_PathfindMoveState(Enemy_Collapse _enemy, Enemy_Collapse_StateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        SMN = enemy.StrictMoveNodes[enemy.strictMoveProcess % enemy.StrictMoveNodes.Count];
        enemy.GazePoint.position = SMN.node.position;
        enemy.target = SMN.node;
    }

    public override void Update()
    {
        base.Update();
        if (Vector3.Distance(enemy.Mover.position, enemy.MovePoint.transform.position) >= .05f)
        {
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

            if (enemy.AreaDetectTarget(enemy.followRadius).Length > 0)
            {
                enemy.StateMachine.ChangeState(enemy.AggroMoveState);
            }
            else if (Vector3.Distance(enemy.Mover.position, SMN.node.position) >= .05f)
            {
                if (enemy.SetPath() < 1) // if there are no path to destination go find other destination
                {
                    enemy.StateMachine.ChangeState(enemy.IdleState);
                }
                else // else, get to next movement
                {
                    enemy.moveDir = enemy.SetVectorOne(new Vector2(enemy.PathList[1].x, enemy.PathList[1].y));

                    if (!enemy.MovepointAdjustCheck())
                        enemy.MovePoint.transform.position += enemy.moveDir;
                }
            }
            else
            {
                enemy.StateMachine.ChangeState(enemy.IdleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.Mover.position = enemy.MovePoint.transform.position;
    }
}
