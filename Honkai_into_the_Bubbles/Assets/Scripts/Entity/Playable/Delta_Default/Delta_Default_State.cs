using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Default_State
{
    protected Delta_Default_StateMachine stateMachine;
    protected Delta_Default player;
    protected string animBoolName;

    protected Vector2 moveInput;
    protected Vector3 savedInput;

    protected float stateTimer;

    protected bool triggerCalled;

    public Delta_Default_State(Delta_Default _player, Delta_Default_StateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.Animator.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        moveInput = InputManager.instance.MoveInput;
        moveInput = new Vector2
        (
            moveInput.x == 0 ? 0 : Mathf.Sign(moveInput.x),
            moveInput.y == 0 ? 0 : Mathf.Sign(moveInput.y)
        );

        if (InputManager.instance.SprintInput)
            player.SetSpeedSprint();
        else
            player.SetSpeedDefault();

        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        player.Animator.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    // check wall and adjust position of movepoint
    protected bool MovepointAdjustCheck()
    {
        Vector3 InputX = new(savedInput.x, 0, 0);
        Vector3 InputY = new(0, savedInput.y, 0);

        if (Physics2D.OverlapCircle(player.MovePoint.transform.position + savedInput, .4f, 1 << LayerMask.NameToLayer("MovepointAdjust")))
        {
            if (savedInput.x == 0 || savedInput.y == 0)
            {
                return false;
            }
            else
            {
                if (Physics2D.OverlapCircle(player.MovePoint.transform.position + InputX, .4f, player.wallLayer))
                    savedInput.x = 0;

                if (Physics2D.OverlapCircle(player.MovePoint.transform.position + InputY, .4f, player.wallLayer))
                    savedInput.y = 0;

                if (Physics2D.OverlapCircle(player.MovePoint.transform.position + savedInput, .4f, 1 << LayerMask.NameToLayer("MovepointAdjust")))
                    return false;
            }
        }

        if (savedInput.x == 0 || savedInput.y == 0)
        {
            if (Physics2D.OverlapCircle(player.MovePoint.transform.position + savedInput, .4f, player.wallLayer))
                return true;
        }
        else // moveInput.x != 0 && moveInput.y != 0
        {


            if (Physics2D.OverlapCircle(player.MovePoint.transform.position + InputX, .4f, player.wallLayer))
                savedInput.x = 0;

            if (Physics2D.OverlapCircle(player.MovePoint.transform.position + InputY, .4f, player.wallLayer))
                savedInput.y = 0;

            if (savedInput == Vector3.zero)
                return true;

            if (savedInput.x != 0 && savedInput.y != 0)
                if (Physics2D.OverlapCircle(player.MovePoint.transform.position + savedInput, .4f, player.wallLayer))
                    player.MovePoint.transform.position -= InputY;
        }
        return false;
    }

    public void SetDir(Vector2 _dir)
    {
        if (_dir.x != 0)
        {
            player.Animator.SetFloat("dirX", _dir.x);
            player.Animator.SetFloat("dirY", 0);
        }
        else
        {
            player.Animator.SetFloat("dirX", 0);
            player.Animator.SetFloat("dirY", _dir.y);
        }
    }
}
