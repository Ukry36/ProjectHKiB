using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_R_SkillState : Delta_R_State
{
    private float lastedTime;
    private bool startAtCombo4;
    public Skill skill;
    bool keepSkill = false;
    private bool stuckCheck = true;

    public Delta_R_SkillState(Delta_R _player, Delta_R_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stuckCheck = true;
        lastedTime = 0;
        startAtCombo4 = false;
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

                // save moveinput
                player.savedInput = (Vector3)player.moveInput;

                // if there is wall, exit walkin
                // else, adjust savedInput or 
                player.SetAnimDir(player.savedInput);
                if (!player.MovepointAdjustCheck())
                {
                    player.MovePoint.transform.position += player.savedInput;
                    player.SetAnimDir(player.savedInput);
                }
            }
        }

        if (!InputManager.instance.DodgeInput && lastedTime <= skill.Cooltime)
        {
            lastedTime += Time.deltaTime;
            if (InputManager.instance.AttackInput)
            {
                startAtCombo4 = true;
            }
            if (InputManager.instance.GraffitiStartInput)
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
            if (startAtCombo4)
            {
                player.AttackState.combo = 3;
                stateMachine.ChangeState(player.AttackState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }

    }

    public override void Exit()
    {
        player.AttractorPrefab.SetActive(false);
        player.DefaultSpeed *= 2;
        base.Exit();
    }
}
