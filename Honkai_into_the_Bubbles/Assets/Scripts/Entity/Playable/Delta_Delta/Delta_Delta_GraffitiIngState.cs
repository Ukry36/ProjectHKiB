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
        player.cannotDodgeState = true;
        player.cannotGraffitiState = true;
    }

    public override void Update()
    {
        base.Update();
        if (unscaledStateTimer < 0 || InputManager.instance.GraffitiEndInput)
        {
            stateMachine.ChangeState(player.GraffitiExitState);
        }
        else if (player.theStat.CurrentGP > 0)
        {
            /*if (player.moveInput == Vector2.zero)
            {
                graffitiSavedInput = player.moveInput;
            }
            else if (graffitiSavedInput == Vector3.zero)
            {
                graffitiSavedInput = player.moveInput;
                player.moveDir = player.moveInput;
                if (graffitiSavedInput.x != 0)
                    graffitiSavedInput.y = 0;
                if (!Physics2D.OverlapCircle(player.MovePoint.transform.position + graffitiSavedInput,
                    0.4f, LayerManager.instance.graffitiWallLayer))
                {
                    player.MovePoint.transform.position += graffitiSavedInput;
                    player.Mover.position = player.MovePoint.transform.position;
                    player.theStat.GPControl(-1, _silence: true);
                }
            }*/

            if (player.moveInputPressed)
            {
                player.moveDir = DInput ? Vector3.down : RInput ? Vector3.right
                               : UInput ? Vector3.up : LInput ? Vector3.left : Vector3.zero;


                if (!player.PointWallCheckForGraffiti(player.Mover.position + player.moveDir))
                {
                    if (!player.endAtGraffitiStartPoint)
                        player.MovePoint.transform.position += player.moveDir;

                    player.Mover.position += player.moveDir;
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.theStat.superArmor = false;
        player.cannotDodgeState = false;
        player.cannotGraffitiState = false;
        player.Mover.position = player.MovePoint.transform.position;
        MenuManager.instance.GraffitiCountDownDisable();
    }
}
