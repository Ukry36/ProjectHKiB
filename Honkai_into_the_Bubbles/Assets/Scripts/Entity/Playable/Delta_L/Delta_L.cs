using System;
using Unity.Mathematics;
using UnityEngine;

public class Delta_L : Playable
{
    public Delta_L_IdleState IdleState { get; private set; }
    public Delta_L_WalkState WalkState { get; private set; }
    public Delta_L_DodgeState DodgeState { get; private set; }
    public Delta_L_KeepDodgeState KeepDodgeState { get; private set; }
    public Delta_L_GraffitiEnterState GraffitiEnterState { get; private set; }
    public Delta_L_GraffitiIngState GraffitiState { get; private set; }
    public Delta_L_GraffitiExitState GraffitiExitState { get; private set; }
    public Delta_L_KnockbackState KnockbackState { get; private set; }
    public Delta_L_BasicAttackState AttackState { get; private set; }
    public Delta_L_BasicAttackExitState AttackExitState { get; private set; }
    public Delta_L_Skill01BeforeState Skill01BeforeState { get; private set; }
    public Delta_L_Skill01IngState Skill01IngState { get; private set; }
    public Delta_L_Skill01AfterState Skill01AfterState { get; private set; }
    public Delta_L_Skill02BeforeState Skill02BeforeState { get; private set; }
    public Delta_L_Skill02SuccessState Skill02SuccessState { get; private set; }
    public Delta_L_Skill02FailState Skill02FailState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        IdleState = new Delta_L_IdleState(this, stateMachine, "Idle", this);
        WalkState = new Delta_L_WalkState(this, stateMachine, "Walk", this);
        DodgeState = new Delta_L_DodgeState(this, stateMachine, "DodgeIng", this);
        KeepDodgeState = new Delta_L_KeepDodgeState(this, stateMachine, "DodgeIng", this);
        GraffitiEnterState = new Delta_L_GraffitiEnterState(this, stateMachine, "DodgeEnter", this);
        GraffitiState = new Delta_L_GraffitiIngState(this, stateMachine, "DodgeIng", this);
        GraffitiExitState = new Delta_L_GraffitiExitState(this, stateMachine, "DodgeExit", this);
        KnockbackState = new Delta_L_KnockbackState(this, stateMachine, "Knockback", this);
        AttackState = new Delta_L_BasicAttackState(this, stateMachine, "Attack", this);
        AttackExitState = new Delta_L_BasicAttackExitState(this, stateMachine, "AttackExit", this);
        Skill01BeforeState = new Delta_L_Skill01BeforeState(this, stateMachine, "Skill01Before", this);
        Skill01IngState = new Delta_L_Skill01IngState(this, stateMachine, "Skill01Ing", this);
        Skill01AfterState = new Delta_L_Skill01AfterState(this, stateMachine, "Skill01After", this);
        Skill02BeforeState = new Delta_L_Skill02BeforeState(this, stateMachine, "Skill02Before", this);
        Skill02SuccessState = new Delta_L_Skill02SuccessState(this, stateMachine, "Skill02Success", this);
        Skill02FailState = new Delta_L_Skill02FailState(this, stateMachine, "Skill02Fail", this);
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
            if (!isDodgeCooltime && !cannotGraffitiState && InputManager.instance.DodgeInput)
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

    public override void SkillManage(int[] _result)
    {
        base.SkillManage(_result);

        switch (_result[0])
        {
            case 0:
                Skill01IngState.skill = GS.skillList[0];
                stateMachine.ChangeState(Skill01BeforeState);
                GraffitiHPHeal(_result[1]);
                break;
            case 1:
                Skill02BeforeState.skill = GS.skillList[1];
                Skill02SuccessState.skill = GS.skillList[1];
                stateMachine.ChangeState(Skill02BeforeState);
                break;
            default:

                stateMachine.ChangeState(IdleState);
                break;
        }

    }

    public override void Hit(Vector3 _attackOrigin)
    {
        stateMachine.currentState.Hit(_attackOrigin);
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

    public override void AnimationControlTrigger()
    {
        stateMachine.currentState.AnimationControlTrigger();
    }
}
