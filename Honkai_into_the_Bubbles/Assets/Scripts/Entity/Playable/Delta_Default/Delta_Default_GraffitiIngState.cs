using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Default_GraffitiIngState : Delta_Default_State
{
    public Delta_Default_GraffitiIngState(Delta_Default _player, Delta_Default_StateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
                player.savedInput = player.moveInput;
            }
            else if (player.savedInput == Vector3.zero)
            {
                player.savedInput = player.moveInput;
                if (player.savedInput.x != 0)
                    player.savedInput.y = 0;
                if (!Physics2D.OverlapCircle(player.MovePoint.transform.position + player.savedInput,
                    0.4f, LayerManager.instance.graffitiWallLayer))
                {
                    player.MovePoint.transform.position += player.savedInput;
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
