using System;
using Unity.Mathematics;
using UnityEngine;

public class Delta_R : Playable
{
    public Delta_R_StateMachine StateMachine { get; private set; }
    public Delta_R_IdleState IdleState { get; private set; }
    public Delta_R_WalkState WalkState { get; private set; }
    public Delta_R_DodgeEnterState DodgeEnterState { get; private set; }
    public Delta_R_DodgeIngState DodgeState { get; private set; }
    public Delta_R_DodgeExitState DodgeExitState { get; private set; }
    public Delta_R_GraffitiEnterState GraffitiEnterState { get; private set; }
    public Delta_R_GraffitiIngState GraffitiState { get; private set; }
    public Delta_R_GraffitiExitState GraffitiExitState { get; private set; }
    public Delta_R_KnockbackState KnockbackState { get; private set; }
    public Delta_R_BasicAttackState AttackState { get; private set; }
    public Delta_R_BasicAttackExitState AttackExitState { get; private set; }
    public Delta_R_Skill01State Skill01State { get; private set; }
    public Delta_R_Skill02State Skill02State { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new Delta_R_StateMachine();

        IdleState = new Delta_R_IdleState(this, StateMachine, "Idle");
        WalkState = new Delta_R_WalkState(this, StateMachine, "Walk");
        DodgeEnterState = new Delta_R_DodgeEnterState(this, StateMachine, "DodgeEnter");
        DodgeState = new Delta_R_DodgeIngState(this, StateMachine, "DodgeIng");
        DodgeExitState = new Delta_R_DodgeExitState(this, StateMachine, "DodgeExit");
        GraffitiEnterState = new Delta_R_GraffitiEnterState(this, StateMachine, "DodgeEnter");
        GraffitiState = new Delta_R_GraffitiIngState(this, StateMachine, "DodgeIng");
        GraffitiExitState = new Delta_R_GraffitiExitState(this, StateMachine, "DodgeExit");
        KnockbackState = new Delta_R_KnockbackState(this, StateMachine, "Knockback");
        AttackState = new Delta_R_BasicAttackState(this, StateMachine, "Attack");
        AttackExitState = new Delta_R_BasicAttackExitState(this, StateMachine, "AttackExit");
        Skill01State = new Delta_R_Skill01State(this, StateMachine, "Skill01");
        Skill02State = new Delta_R_Skill02State(this, StateMachine, "Skill02");
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
