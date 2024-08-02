using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_L_GraffitiIngState : Delta_L_State
{
    private Vector3 graffitiSavedInput;
    public Delta_L_GraffitiIngState(Delta_L _player, Delta_L_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.cannotDodge = true;
        player.cannotGraffiti = true;
        stateTimer = player.graffitiMaxtime + PlayerManager.instance.exGraffitimaxtime;
        MenuManager.instance.GraffitiCountDownEnable(stateTimer);
        player.theStat.superArmor = true;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0 || InputManager.instance.GraffitiEndInput)
        {
            player.StateMachine.ChangeState(player.GraffitiExitState);
        }
        else if (player.theStat.currentGP > 0)
        {
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
    }

    public override void Exit()
    {
        base.Exit();
        player.cannotDodge = false;
        player.cannotGraffiti = false;
        player.theStat.superArmor = false;
    }
}
