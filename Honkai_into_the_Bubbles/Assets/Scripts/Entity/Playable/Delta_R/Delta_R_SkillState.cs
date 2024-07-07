using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_R_SkillState : Delta_R_State
{
    public Skill skill;
    bool keepSkill = false;
    private bool stuckCheck = true;

    public Delta_R_SkillState(Delta_R _player, Delta_R_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.theStat.superArmor = true;
        stuckCheck = true;
        stateTimer = skill.Cooltime;
        keepSkill = false;
        player.AttractorPrefab.SetActive(true);
        player.DefaultSpeed /= 2;
    }

    public override void Update()
    {
        base.Update();

        if (Vector3.Distance(player.Mover.position, player.MovePoint.transform.position) >= .05f)
        {
            if (stuckCheck)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(player.MovePoint.transform.position, 0.1f, player.NoMovepointWallLayer);
                if (colliders != null && colliders.Length > 0)
                { player.MovePoint.transform.position = player.MovePoint.prevPos; Debug.Log("stuck"); }
            }
            stuckCheck = false;

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
            stuckCheck = true;
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

        if (!InputManager.instance.DodgeInput && stateTimer > 0)
        {
            if (InputManager.instance.AttackInput)
            {
                player.skill01ing = false;
                player.skill02ing = false;
                if (player.moveInput != Vector2.zero)
                    player.savedInput = player.moveInput;
                player.AttackState.combo = 3;
                stateMachine.ChangeState(player.AttackState);
            }
            else if (InputManager.instance.GraffitiStartInput)
            {
                keepSkill = true;
            }
        }
        else
        {
            if (!keepSkill)
            {
                player.skill01ing = false;
                player.skill02ing = false;
            }
            stateMachine.ChangeState(player.IdleState);
        }

    }

    public override void Exit()
    {
        base.Exit();
        player.theStat.superArmor = false;
        player.Mover.position = player.MovePoint.transform.position;
        player.AttractorPrefab.SetActive(false);
        player.DefaultSpeed *= 2;

    }
}
