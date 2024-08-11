using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_L_BasicAttackState : Playable_State
{
    public int combo = 0;
    private bool attackReserved;
    private Delta_L player;
    public Delta_L_BasicAttackState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_L _player) : base(_player, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        player.Animator.SetInteger("Combo", combo);
        player.SetAnimDir(player.savedInput);
        if (player.moveInput != Vector2.zero)
        {
            player.savedInput = player.moveInput;
            for (int i = 0; i < player.AttackArray[combo].TrackingRadius; i++)
            {
                if (!player.MovepointAdjustCheck())
                {
                    player.MovePoint.transform.position += player.savedInput;
                    player.Mover.position = player.MovePoint.transform.position;
                }
            }
        }
    }

    public override void Update()
    {
        base.Update();
        if (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f)
        {
            if (InputManager.instance.AttackInput)
                attackReserved = true;

            if (player.moveInput != Vector2.zero)
                player.savedInput = player.moveInput;
        }
        if (finishTriggerCalled)
        {
            player.AttackExitState.attackReserved = attackReserved;

            player.AttackExitState.combo = combo;
            stateMachine.ChangeState(player.AttackExitState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        attackReserved = false;
        combo = 0;
    }
}
