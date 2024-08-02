using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_L_Skill01IngState : Delta_L_State
{
    public Skill skill;
    public Delta_L_Skill01IngState(Delta_L _player, Delta_L_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.cannotDodge = true;
        player.cannotGraffiti = true;
        player.boxCollider.enabled = false;
        stateTimer = skill.Cooltime;
    }

    public override void Update()
    {
        base.Update();

        if (Vector3.Distance(player.Mover.position, player.MovePoint.transform.position) >= .05f)
        {
            player.Mover.position = Vector3.MoveTowards
            (
                player.Mover.position,
                player.MovePoint.transform.position,
                player.MoveSpeed * Time.deltaTime
            );
        }
        else
        {
            player.Mover.position = player.MovePoint.transform.position; // make position accurate
            player.MovePoint.prevPos = player.Mover.position; // used in external movepoint control
            if (player.moveInput != Vector2.zero)
            {
                player.savedInput = (Vector3)player.moveInput;

                player.SetAnimDir(player.savedInput);
                if (!player.MovepointAdjustCheck())
                {
                    player.MovePoint.transform.position += player.savedInput;
                    player.SetAnimDir(player.savedInput);
                }
            }
        }


        if (InputManager.instance.AttackInput || stateTimer < 0)
        {
            if (player.moveInput != Vector2.zero)
                player.savedInput = player.moveInput;
            stateMachine.ChangeState(player.Skill01AfterState);
        }
    }


    public override void Exit()
    {
        base.Exit();
        player.cannotDodge = false;
        player.cannotGraffiti = false;
        player.boxCollider.enabled = true;
        player.Mover.position = player.MovePoint.transform.position;
    }
}