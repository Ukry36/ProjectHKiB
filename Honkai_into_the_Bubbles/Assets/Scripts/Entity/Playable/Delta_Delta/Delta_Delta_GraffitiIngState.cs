using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Delta_GraffitiIngState : Playable_State
{
    private Vector3 graffitiSavedInput;
    private Delta_Delta player;
    public Delta_Delta_GraffitiIngState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Delta _player) : base(_playerBase, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        unscaledStateTimer = player.graffitiMaxtime + PlayerManager.instance.exGraffitimaxtime;
        MenuManager.instance.GraffitiCountDownEnable(unscaledStateTimer);
        player.theStat.superArmor = true;
    }

    public override void Update()
    {
        base.Update();
        if (unscaledStateTimer < 0 || InputManager.instance.GraffitiEndInput)
        {
            stateMachine.ChangeState(player.GraffitiExitState);
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
                    player.theStat.GPControl(-1, _silence: true);
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.theStat.superArmor = false;
    }
}
