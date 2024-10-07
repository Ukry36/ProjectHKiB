using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Default : Playable
{
    public Delta_Default_IdleState IdleState { get; protected set; }
    public Delta_Default_WalkState WalkState { get; protected set; }
    public Delta_Default_DodgeState DodgeState { get; protected set; }
    public Delta_Default_KeepDodgeState KeepDodgeState { get; protected set; }
    public Delta_Default_GraffitiEnterState GraffitiEnterState { get; protected set; }
    public Delta_Default_GraffitiIngState GraffitiState { get; protected set; }
    public Delta_Default_GraffitiExitState GraffitiExitState { get; protected set; }
    public Delta_Default_KnockbackState KnockbackState { get; protected set; }

    protected override void Awake()
    {
        base.Awake();

        IdleState = new Delta_Default_IdleState(this, stateMachine, "Idle", this);
        WalkState = new Delta_Default_WalkState(this, stateMachine, "Walk", this);
        DodgeState = new Delta_Default_DodgeState(this, stateMachine, "DodgeIng", this);
        KeepDodgeState = new Delta_Default_KeepDodgeState(this, stateMachine, "DodgeIng", this);
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
            if (!isDodgeCooltime && !cannotDodgeState && InputManager.instance.DodgeInput)
            {
                if (keepDodge || PlayerManager.instance.forcedKeepDodge)
                    stateMachine.ChangeState(KeepDodgeState);
                else
                    stateMachine.ChangeState(DodgeState);
            }

        if (canGraffitiEffect)
            if (!isGraffitiCooltime && !cannotGraffitiState && InputManager.instance.GraffitiStartInput && theStat.CurrentGP > 0)
            {
                MovePoint.gameObject.SetActive(false);
                if (!Physics2D.OverlapCircle(MovePoint.transform.position, .4f, LayerManager.instance.graffitiWallLayer))
                {
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
        Vector3 GrrogyDir = theStat.transform.position - _attackOrigin;

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
