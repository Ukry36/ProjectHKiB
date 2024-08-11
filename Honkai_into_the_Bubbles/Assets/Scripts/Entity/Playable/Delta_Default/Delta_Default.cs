using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Default : Playable
{
    public Delta_Default_IdleState IdleState { get; protected set; }
    public Delta_Default_WalkState WalkState { get; protected set; }
    public Delta_Default_DodgeEnterState DodgeEnterState { get; protected set; }
    public Delta_Default_DodgeIngState DodgeState { get; protected set; }
    public Delta_Default_DodgeExitState DodgeExitState { get; protected set; }
    public Delta_Default_GraffitiEnterState GraffitiEnterState { get; protected set; }
    public Delta_Default_GraffitiIngState GraffitiState { get; protected set; }
    public Delta_Default_GraffitiExitState GraffitiExitState { get; protected set; }
    public Delta_Default_KnockbackState KnockbackState { get; protected set; }

    protected override void Awake()
    {
        base.Awake();

        IdleState = new Delta_Default_IdleState(this, stateMachine, "Idle", this);
        WalkState = new Delta_Default_WalkState(this, stateMachine, "Walk", this);
        DodgeEnterState = new Delta_Default_DodgeEnterState(this, stateMachine, "DodgeEnter", this);
        DodgeState = new Delta_Default_DodgeIngState(this, stateMachine, "DodgeIng", this);
        DodgeExitState = new Delta_Default_DodgeExitState(this, stateMachine, "DodgeExit", this);
        GraffitiEnterState = new Delta_Default_GraffitiEnterState(this, stateMachine, "DodgeEnter", this);
        GraffitiState = new Delta_Default_GraffitiIngState(this, stateMachine, "DodgeIng", this);
        GraffitiExitState = new Delta_Default_GraffitiExitState(this, stateMachine, "DodgeExit", this);
        KnockbackState = new Delta_Default_KnockbackState(this, stateMachine, "Knockback", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        if (canDodgeEffect || PlayerManager.instance.forcedCanDodge)
            if (!isDodgeCooltime && InputManager.instance.DodgeInput
            && stateMachine.currentState != DodgeState
            && stateMachine.currentState != DodgeEnterState
            && stateMachine.currentState != GraffitiState
            && stateMachine.currentState != GraffitiEnterState
            && stateMachine.currentState != GraffitiExitState)
            {
                dodgeSprite.color = PlayerManager.instance.ThemeColors
                [
                    totalDodgeCount++ % PlayerManager.instance.ThemeColors.Count
                ];
                stateMachine.ChangeState(DodgeEnterState);
            }

        if (canGraffitiEffect)
            if (!isGraffitiCooltime && InputManager.instance.GraffitiStartInput && theStat.currentGP > 0
            && stateMachine.currentState != DodgeEnterState
            && stateMachine.currentState != GraffitiState
            && stateMachine.currentState != GraffitiEnterState
            && stateMachine.currentState != GraffitiExitState)
            {
                MovePoint.gameObject.SetActive(false);
                if (!Physics2D.OverlapCircle(MovePoint.transform.position, .4f, LayerManager.instance.graffitiWallLayer))
                {
                    dodgeSprite.color = PlayerManager.instance.ThemeColors
                    [
                        totalDodgeCount++ % PlayerManager.instance.ThemeColors.Count
                    ];
                    if (stateMachine.currentState == DodgeState)
                        stateMachine.ChangeState(GraffitiState);
                    else
                        stateMachine.ChangeState(GraffitiEnterState);
                }
                MovePoint.gameObject.SetActive(true);

            }
    }

    public override void Knockback(Vector3 _attackOrigin, int _coeff)
    {
        Vector3 GrrogyDir = this.transform.position - _attackOrigin;

        float x = GrrogyDir.x != 0 ? MathF.Sign(GrrogyDir.x) : 0,
              y = GrrogyDir.y != 0 ? MathF.Sign(GrrogyDir.y) : 0;

        if (x * y != 0)
        {
            if (Mathf.Abs(x) >= Mathf.Abs(y) * 2) y = 0;
            if (Mathf.Abs(y) >= Mathf.Abs(x) * 2) x = 0;
        }

        if (x == 0 && y == 0)
        {
            x = MathF.Sign(UnityEngine.Random.Range(-1f, 1f));
            y = MathF.Sign(UnityEngine.Random.Range(-1f, 1f));
        }

        GrrogyDir = new Vector3(x, y, 0);

        KnockbackState.dir = GrrogyDir;
        KnockbackState.coeff = _coeff;
        stateMachine.ChangeState(KnockbackState);
    }

    public override void AnimationFinishTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }
}
