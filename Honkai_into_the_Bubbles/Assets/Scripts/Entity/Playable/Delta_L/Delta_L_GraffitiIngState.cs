using UnityEngine;

public class Delta_L_GraffitiIngState : Playable_State
{
    private Vector3 graffitiSavedInput;
    private Delta_L player;
    public Delta_L_GraffitiIngState(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_L _player) : base(_player, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        player.cannotDodge = true;
        player.cannotGraffiti = true;
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


                if (!Physics2D.OverlapCircle(player.Mover.position + player.moveDir,
                    0.4f, LayerManager.instance.graffitiWallLayer))
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
        player.Mover.position = player.MovePoint.transform.position;
        player.cannotDodge = false;
        player.cannotGraffiti = false;
        player.theStat.superArmor = false;
    }
}
