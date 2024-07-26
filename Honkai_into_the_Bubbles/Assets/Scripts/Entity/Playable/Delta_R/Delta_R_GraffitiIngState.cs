using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_R_GraffitiIngState : Delta_R_State
{
    private Vector3 graffitiSavedInput;
    public Delta_R_GraffitiIngState(Delta_R _player, Delta_R_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.graffitiMaxtime + PlayerManager.instance.exGraffitimaxtime;
        player.theStat.superArmor = true;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0 || InputManager.instance.GraffitiEndInput)
        {
            player.StateMachine.ChangeState(player.GraffitiExitState);
        }


        if (player.theStat.currentGP > 0)
            if (player.moveInput == Vector2.zero)
            {
                graffitiSavedInput = player.moveInput;
            }
            else if (graffitiSavedInput == Vector3.zero)
            {
                graffitiSavedInput = player.moveInput;
                player.savedInput = player.moveInput;
                if (graffitiSavedInput.x != 0)
                    graffitiSavedInput.y = 0;
                if (!Physics2D.OverlapCircle(player.MovePoint.transform.position + graffitiSavedInput,
                    0.4f, LayerManager.instance.graffitiWallLayer))
                {
                    player.MovePoint.transform.position += graffitiSavedInput;
                    player.Mover.position = player.MovePoint.transform.position;
                    player.theStat.GPControl(-1);
                }
            }
    }

    public override void Exit()
    {
        base.Exit();
        player.theStat.superArmor = false;
    }
}
