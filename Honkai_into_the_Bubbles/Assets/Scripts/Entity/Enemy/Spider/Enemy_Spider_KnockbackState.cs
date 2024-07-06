using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spider_KnockbackState : Enemy_Spider_State
{
    public Vector2 dir;
    public int coeff;
    private int i;

    public Enemy_Spider_KnockbackState(Enemy_Spider _enemy, Enemy_Spider_StateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {

    }

    // set initial direction 
    public override void Enter()
    {
        base.Enter();

        enemy.SetAnimDir(-dir);
        i = 0;
    }

    // knockback x coeff -> idlestate
    public override void Update()
    {
        base.Update();

        if (i < coeff)
        {
            enemy.Mover.position = enemy.MovePoint.transform.position;

            if (dir.x == 0 || dir.y == 0)
            {
                if (Physics2D.OverlapCircle(enemy.MovePoint.transform.position + new Vector3(dir.x, dir.y, 0), .4f, enemy.wallLayer))
                    dir = Vector3.zero;
            }
            else
            {
                if (Physics2D.OverlapCircle(enemy.MovePoint.transform.position + new Vector3(dir.x, 0, 0), .4f, enemy.wallLayer))
                {
                    dir.x = 0;
                    enemy.Animator.SetFloat("dirX", 0);
                    enemy.Animator.SetFloat("dirY", -dir.y);
                }

                if (Physics2D.OverlapCircle(enemy.MovePoint.transform.position + new Vector3(0, dir.y, 0), .4f, enemy.wallLayer))
                    dir.y = 0;
            }

            enemy.MovePoint.transform.position += (Vector3)dir;
        }
        else if (Vector3.Distance(enemy.Mover.position, enemy.MovePoint.transform.position) >= .05f)
        {
            enemy.Mover.position = Vector3.MoveTowards(enemy.Mover.position, enemy.MovePoint.transform.position, 4 * Time.deltaTime);
        }
        else
        {
            enemy.Mover.position = enemy.MovePoint.transform.position;
            enemy.StateMachine.ChangeState(enemy.IdleState);
        }

        i++;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
