using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Default_KnockbackState : Playable_State
{
    public Vector2 dir;
    public int coeff;
    private int i;

    private Delta_Default player;
    public Delta_Default_KnockbackState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Default _player) : base(_player, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    // set initial direction 
    public override void Enter()
    {
        base.Enter();

        player.SetAnimDir(-dir);
        i = 0;
    }

    // knockback x coeff -> idlestate
    public override void Update()
    {
        base.Update();

        if (i < coeff)
        {
            player.Mover.position = player.MovePoint.transform.position;

            if (dir.x == 0 || dir.y == 0)
            {
                if (Physics2D.OverlapCircle(player.MovePoint.transform.position + new Vector3(dir.x, dir.y, 0), .4f, player.wallLayer))
                    dir = Vector3.zero;
            }
            else
            {
                if (Physics2D.OverlapCircle(player.MovePoint.transform.position + new Vector3(dir.x, 0, 0), .4f, player.wallLayer))
                {
                    dir.x = 0;
                    player.Animator.SetFloat("dirX", 0);
                    player.Animator.SetFloat("dirY", -dir.y);
                }

                if (Physics2D.OverlapCircle(player.MovePoint.transform.position + new Vector3(0, dir.y, 0), .4f, player.wallLayer))
                    dir.y = 0;
            }

            player.MovePoint.transform.position += (Vector3)dir;
        }
        else if (Vector3.Distance(player.Mover.position, player.MovePoint.transform.position) >= .05f)
        {
            player.Mover.position = Vector3.MoveTowards(player.Mover.position, player.MovePoint.transform.position, 4 * Time.deltaTime);
        }
        else
        {
            player.Mover.position = player.MovePoint.transform.position;
            stateMachine.ChangeState(player.IdleState);
        }

        i++;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
