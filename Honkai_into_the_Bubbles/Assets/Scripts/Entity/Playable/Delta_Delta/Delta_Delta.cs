using System;
using Unity.Mathematics;
using UnityEngine;

public class Delta_Delta : Playable
{
    public Delta_Delta_StateMachine StateMachine { get; private set; }
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
        StateMachine = new Delta_Delta_StateMachine();

        IdleState = new Delta_Delta_IdleState(this, StateMachine, "Idle");
        WalkState = new Delta_Delta_WalkState(this, StateMachine, "Walk");
        DodgeEnterState = new Delta_Delta_DodgeEnterState(this, StateMachine, "DodgeEnter");
        DodgeState = new Delta_Delta_DodgeIngState(this, StateMachine, "DodgeIng");
        DodgeExitState = new Delta_Delta_DodgeExitState(this, StateMachine, "DodgeExit");
        GraffitiEnterState = new Delta_Delta_GraffitiEnterState(this, StateMachine, "DodgeEnter");
        GraffitiState = new Delta_Delta_GraffitiIngState(this, StateMachine, "DodgeIng");
        GraffitiExitState = new Delta_Delta_GraffitiExitState(this, StateMachine, "DodgeExit");
        KnockbackState = new Delta_Delta_KnockbackState(this, StateMachine, "Knockback");
        AttackState = new Delta_Delta_BasicAttackState(this, StateMachine, "Attack");
        AttackExitState = new Delta_Delta_BasicAttackExitState(this, StateMachine, "AttackExit");
        Skill01State = new Delta_Delta_Skill01State(this, StateMachine, "Skill01");
        Skill02State = new Delta_Delta_Skill02State(this, StateMachine, "Skill02");
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        StateMachine.currentState.Update();
        if (canDodgeEffect || PlayerManager.instance.forcedCanDodge)
            if (!isDodgeCooltime && InputManager.instance.DodgeInput
            && StateMachine.currentState != DodgeState
            && StateMachine.currentState != DodgeEnterState
            && StateMachine.currentState != GraffitiState
            && StateMachine.currentState != GraffitiEnterState
            && StateMachine.currentState != GraffitiExitState)
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
                if (!Physics2D.OverlapCircle(MovePoint.transform.position, .4f, LayerManager.instance.graffitiWallLayer))
                {
                    Debug.Log("sccs");
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

    public override void SkillManage(int _skillNum)
    {
        base.SkillManage(_skillNum);
        StateMachine.ChangeState(IdleState);
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
