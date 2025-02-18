using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_R_SkillState : Playable_State
{
    public Skill skill;
    bool keepSkill = false;
    private Delta_R player;
    public Delta_R_SkillState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_R _player) : base(_playerBase, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        player.theStat.superArmor = true;
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
                player.moveDir = (Vector3)player.moveInput;

                player.SetAnimDir(player.moveDir);
                if (!player.MovepointAdjustCheck())
                {
                    player.MovePoint.transform.position += player.moveDir;
                    player.SetAnimDir(player.moveDir);
                }
            }
        }

        if (InputManager.instance.DodgeInput || stateTimer < 0)
        {
            if (!keepSkill)
            {
                if (player.skill02ing)
                    player.theStat.ATKBuff -= 50;
                player.skill01ing = false;
                player.skill02ing = false;
            }
            stateMachine.ChangeState(player.IdleState);

        }
        else if (InputManager.instance.AttackInput)
        {
            if (player.skill02ing)
                player.theStat.ATKBuff -= 50;
            player.skill01ing = false;
            player.skill02ing = false;
            if (player.moveInput != Vector2.zero)
                player.moveDir = player.moveInput;
            player.AttackState.combo = 3;
            stateMachine.ChangeState(player.AttackState);
        }
        else if (InputManager.instance.GraffitiStartInput)
        {
            keepSkill = true;
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
