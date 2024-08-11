using System;
using Unity.Mathematics;
using UnityEngine;

public class Delta_Delta : Playable
{
    public Delta_Delta_IdleState IdleState { get; private set; }
    public Delta_Delta_WalkState WalkState { get; private set; }
    public Delta_Delta_DodgeEnterState DodgeEnterState { get; private set; }
    public Delta_Delta_DodgeIngState DodgeState { get; private set; }
    public Delta_Delta_DodgeExitState DodgeExitState { get; private set; }
    public Delta_Delta_GraffitiEnterState GraffitiEnterState { get; private set; }
    public Delta_Delta_GraffitiIngState GraffitiState { get; private set; }
    public Delta_Delta_GraffitiExitState GraffitiExitState { get; private set; }
    public Delta_Delta_KnockbackState KnockbackState { get; private set; }
    public Delta_Delta_BasicAttackState AttackState { get; private set; }
    public Delta_Delta_BasicAttackExitState AttackExitState { get; private set; }
    public Delta_Delta_Skill01State Skill01State { get; private set; }
    public Delta_Delta_Skill02State Skill02State { get; private set; }

    public bool startAtCombo3;

    protected override void Awake()
    {
        base.Awake();
        IdleState = new Delta_Delta_IdleState(this, stateMachine, "Idle", this);
        WalkState = new Delta_Delta_WalkState(this, stateMachine, "Walk", this);
        DodgeEnterState = new Delta_Delta_DodgeEnterState(this, stateMachine, "DodgeEnter", this);
        DodgeState = new Delta_Delta_DodgeIngState(this, stateMachine, "DodgeIng", this);
        DodgeExitState = new Delta_Delta_DodgeExitState(this, stateMachine, "DodgeExit", this);
        GraffitiEnterState = new Delta_Delta_GraffitiEnterState(this, stateMachine, "DodgeEnter", this);
        GraffitiState = new Delta_Delta_GraffitiIngState(this, stateMachine, "DodgeIng", this);
        GraffitiExitState = new Delta_Delta_GraffitiExitState(this, stateMachine, "DodgeExit", this);
        KnockbackState = new Delta_Delta_KnockbackState(this, stateMachine, "Knockback", this);
        AttackState = new Delta_Delta_BasicAttackState(this, stateMachine, "Attack", this);
        AttackExitState = new Delta_Delta_BasicAttackExitState(this, stateMachine, "AttackExit", this);
        Skill01State = new Delta_Delta_Skill01State(this, stateMachine, "Skill01", this);
        Skill02State = new Delta_Delta_Skill02State(this, stateMachine, "Skill02", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        stateMachine.currentState.Update();
        if (canDodgeEffect || PlayerManager.instance.forcedCanDodge)
            if (!isDodgeCooltime && InputManager.instance.DodgeInput
            && stateMachine.currentState != DodgeState
            && stateMachine.currentState != DodgeEnterState
            && stateMachine.currentState != GraffitiState
            && stateMachine.currentState != GraffitiEnterState
            && stateMachine.currentState != GraffitiExitState)
            {
                if (AttackState.combo == 2 || AttackState.combo == 3
                    || AttackExitState.combo == 2 || AttackExitState.combo == 3)
                {
                    startAtCombo3 = true;
                }
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

    public override void SkillManage(int[] _skillNum)
    {
        base.SkillManage(_skillNum);
        stateMachine.ChangeState(IdleState);
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
