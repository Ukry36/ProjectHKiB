using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_R_SkillState : Delta_R_State
{
    private float lastedTime;
    private bool startAtCombo4;
    public Skill skill;
    bool keepSkill = false;

    public Delta_R_SkillState(Delta_R _player, Delta_R_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        lastedTime = 0;
        startAtCombo4 = false;
        player.AttractorPrefab.SetActive(true);
        player.DefaultSpeed /= 2;
    }

    public override void Update()
    {
        base.Update();

        //attack.SetAttackInfo(_skill.DamageCoefficient, _skill.BaseCriticalRate, _skill.Strong > 0);

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
            player.AttractorPrefab.SetActive(false);
            player.DefaultSpeed *= 2;
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
        base.Exit();
    }
}
