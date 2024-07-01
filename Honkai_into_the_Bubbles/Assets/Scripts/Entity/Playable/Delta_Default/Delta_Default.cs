using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Default : Playable
{
    public Delta_Default_StateMachine StateMachine { get; private set; }
    public Delta_Default_IdleState IdleState { get; private set; }
    public Delta_Default_WalkState WalkState { get; private set; }
    public Delta_Default_DodgeEnterState DodgeEnterState { get; private set; }
    public Delta_Default_DodgeIngState DodgeState { get; private set; }
    public Delta_Default_DodgeExitState DodgeExitState { get; private set; }
    public Delta_Default_GraffitiEnterState GraffitiEnterState { get; private set; }
    public Delta_Default_GraffitiIngState GraffitiState { get; private set; }
    public Delta_Default_GraffitiExitState GraffitiExitState { get; private set; }
    public Delta_Default_KnockbackState KnockbackState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new Delta_Default_StateMachine();

        IdleState = new Delta_Default_IdleState(this, StateMachine, "Idle");
        WalkState = new Delta_Default_WalkState(this, StateMachine, "Walk");
        DodgeEnterState = new Delta_Default_DodgeEnterState(this, StateMachine, "DodgeEnter");
        DodgeState = new Delta_Default_DodgeIngState(this, StateMachine, "DodgeIng");
        DodgeExitState = new Delta_Default_DodgeExitState(this, StateMachine, "DodgeExit");
        GraffitiEnterState = new Delta_Default_GraffitiEnterState(this, StateMachine, "DodgeEnter");
        GraffitiState = new Delta_Default_GraffitiIngState(this, StateMachine, "DodgeIng");
        GraffitiExitState = new Delta_Default_GraffitiExitState(this, StateMachine, "DodgeExit");
        KnockbackState = new Delta_Default_KnockbackState(this, StateMachine, "Knockback");
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.currentState.Update();
        if (canDodgeEffect || PlayerManager.instance.forcedCanDodge)
            if (!isDodgeCooltime && InputManager.instance.DodgeInput
            && StateMachine.currentState != DodgeState
            && StateMachine.currentState != DodgeEnterState
            && StateMachine.currentState != GraffitiState
            && StateMachine.currentState != GraffitiEnterState
            && StateMachine.currentState != GraffitiExitState)
            {
                dodgeSprite.color = PlayerManager.instance.ThemeColors
                [
                    totalDodgeCount++ % PlayerManager.instance.ThemeColors.Count
                ];
                StateMachine.ChangeState(DodgeEnterState);
            }

        if (canGraffitiEffect)
            if (!isGraffitiCooltime && InputManager.instance.GraffitiStartInput && theStat.currentGP > 0
            && StateMachine.currentState != DodgeEnterState
            && StateMachine.currentState != GraffitiState
            && StateMachine.currentState != GraffitiEnterState
            && StateMachine.currentState != GraffitiExitState)
            {
                MovePoint.gameObject.SetActive(false);
                if (!Physics2D.OverlapCircle(MovePoint.transform.position, .4f, wallLayer + GS.WallForGraffitiLayer))
                {
                    dodgeSprite.color = PlayerManager.instance.ThemeColors
                    [
                        totalDodgeCount++ % PlayerManager.instance.ThemeColors.Count
                    ];
                    theStat.GPControl(-1);
                    if (StateMachine.currentState == DodgeState)
                        StateMachine.ChangeState(GraffitiState);
                    else
                        StateMachine.ChangeState(GraffitiEnterState);
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
        StateMachine.ChangeState(KnockbackState);
    }
}
