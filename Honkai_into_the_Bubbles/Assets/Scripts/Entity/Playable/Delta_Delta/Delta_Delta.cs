using System;
using System.Collections;
using NaughtyAttributes;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Delta_Delta : Playable
{
    public Delta_Delta_IdleState IdleState { get; private set; }
    public Delta_Delta_WalkState WalkState { get; private set; }
    public Delta_Delta_DodgeState DodgeState { get; private set; }
    public Delta_Delta_KeepDodgeState KeepDodgeState { get; private set; }
    public Delta_Delta_GraffitiEnterState GraffitiEnterState { get; private set; }
    public Delta_Delta_GraffitiIngState GraffitiState { get; private set; }
    public Delta_Delta_GraffitiExitState GraffitiExitState { get; private set; }
    public Delta_Delta_KnockbackState KnockbackState { get; private set; }
    public Delta_Delta_BasicAttackState AttackState { get; private set; }
    public Delta_Delta_BasicAttackExitState AttackExitState { get; private set; }
    public Delta_Delta_BurstAttackState BurstAttackState { get; private set; }
    public Delta_Delta_BurstAttackExitState BurstAttackExitState { get; private set; }
    public Delta_Delta_ChargeSkillState ChargeSkillState { get; private set; }
    public Delta_Delta_Skill01BeforeState Skill01BeforeState { get; private set; }
    public Delta_Delta_Skill01SuccessState Skill01SuccessState { get; private set; }
    public Delta_Delta_Skill01FailState Skill01FailState { get; private set; }
    public Delta_Delta_Skill01FailEndState Skill01FailEndState { get; private set; }
    public Delta_Delta_Skill02State Skill02State { get; private set; }

    [HideInInspector] public bool startAtCombo3;

    private int dodgeGuage;
    private int attackGuage;
    [HideInInspector] public bool canCharge;
    /*[HideInInspector]*/
    public bool isBurstMode;
    private float burstTimer;
    [BoxGroup("Attack")]
    public Attack ChargeSkill;
    [BoxGroup("Attack")]
    public Attack[] BurstAttackArray;
    [BoxGroup("Attack")]
    public float burstModeMaxTime;


    [BoxGroup("UI")]
    [SerializeField] private GameObject guageUIGameObject;
    [BoxGroup("UI")]
    [SerializeField] private Image leftDodgeGuageUI;
    [BoxGroup("UI")]
    [SerializeField] private Image rightAttackGuageUI;
    [BoxGroup("UI")]
    [SerializeField] private GameObject chargeActivatedUI;

    protected override void Awake()
    {
        base.Awake();
        IdleState = new Delta_Delta_IdleState(this, stateMachine, "Idle", this);
        WalkState = new Delta_Delta_WalkState(this, stateMachine, "Walk", this);
        DodgeState = new Delta_Delta_DodgeState(this, stateMachine, "DodgeIng", this);
        KeepDodgeState = new Delta_Delta_KeepDodgeState(this, stateMachine, "DodgeIng", this);
        GraffitiEnterState = new Delta_Delta_GraffitiEnterState(this, stateMachine, "DodgeEnter", this);
        GraffitiState = new Delta_Delta_GraffitiIngState(this, stateMachine, "DodgeIng", this);
        GraffitiExitState = new Delta_Delta_GraffitiExitState(this, stateMachine, "DodgeExit", this);
        KnockbackState = new Delta_Delta_KnockbackState(this, stateMachine, "Knockback", this);
        AttackState = new Delta_Delta_BasicAttackState(this, stateMachine, "Attack", this);
        AttackExitState = new Delta_Delta_BasicAttackExitState(this, stateMachine, "AttackExit", this);
        BurstAttackState = new Delta_Delta_BurstAttackState(this, stateMachine, "Attack", this);
        BurstAttackExitState = new Delta_Delta_BurstAttackExitState(this, stateMachine, "AttackExit", this);
        ChargeSkillState = new Delta_Delta_ChargeSkillState(this, stateMachine, "Charge", this);
        Skill01BeforeState = new Delta_Delta_Skill01BeforeState(this, stateMachine, "Skill01Before", this);
        Skill01SuccessState = new Delta_Delta_Skill01SuccessState(this, stateMachine, "Skill01Success", this);
        Skill01FailState = new Delta_Delta_Skill01FailState(this, stateMachine, "Skill01Success", this);
        Skill01FailEndState = new Delta_Delta_Skill01FailEndState(this, stateMachine, "Skill01Fail", this);
        Skill02State = new Delta_Delta_Skill02State(this, stateMachine, "Skill02", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(IdleState);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        guageUIGameObject.SetActive(true);
    }

    protected virtual void OnDisable()
    {
        guageUIGameObject.SetActive(false);
        if (isBurstMode)
            EndBurstMode();
    }

    protected override void Update()
    {
        stateMachine.currentState.Update();
        if (canDodgeEffect || PlayerManager.instance.forcedCanDodge)
            if (!isDodgeCooltime && !cannotDodgeState && InputManager.instance.DodgeInput)
            {
                if (AttackState.combo == 2 || AttackState.combo == 3
                    || AttackExitState.combo == 2 || AttackExitState.combo == 3)
                {
                    StopCoroutine(nameof(StartAtCombo3Delay));
                    startAtCombo3 = true;
                }

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

        if (isBurstMode && burstTimer >= 0)
        {
            burstTimer -= Time.deltaTime;
            leftDodgeGuageUI.fillAmount = burstTimer / burstModeMaxTime * 0.5f;
            rightAttackGuageUI.fillAmount = burstTimer / burstModeMaxTime * 0.5f;
            leftDodgeGuageUI.fillAmount = leftDodgeGuageUI.fillAmount > 0.5f ? 0.5f : leftDodgeGuageUI.fillAmount;
            rightAttackGuageUI.fillAmount = rightAttackGuageUI.fillAmount > 0.5f ? 0.5f : rightAttackGuageUI.fillAmount;

            if (burstTimer < 0)
            {
                EndBurstMode();
            }
        }
    }

    public void DodgeGuageManage(int _amount)
    {
        if (!isBurstMode)
        {
            dodgeGuage = _amount > 0 ? (dodgeGuage + _amount > 100 ? 100 : dodgeGuage + _amount) : 0;
            UpdateGuage();
        }

    }

    public void AttackGuageManage(int _amount)
    {
        if (!isBurstMode)
        {
            attackGuage = _amount > 0 ? (attackGuage + _amount > 100 ? 100 : attackGuage + _amount) : 0;
            UpdateGuage();
        }
    }

    public void UpdateGuage()
    {
        if (!isBurstMode)
        {
            leftDodgeGuageUI.fillAmount = (float)dodgeGuage / 100 * 0.5f;
            rightAttackGuageUI.fillAmount = (float)attackGuage / 100 * 0.5f;
            canCharge = dodgeGuage == 100 && attackGuage == 100;
        }
        chargeActivatedUI.SetActive(canCharge || isBurstMode);
    }

    public void StartBurstMode(int _coeff)
    {
        isBurstMode = true;
        burstTimer = burstModeMaxTime * _coeff;
        dodgeGuage = 0;
        attackGuage = 0;
        canCharge = false;
        theStat.ATKBuff += 50;
        Animator.SetBool("Burst", true);
    }

    public void FillAll()
    {
        dodgeGuage = 100;
        attackGuage = 100;
        UpdateGuage();
    }

    public void MaintainBurstMode(int _coeff)
    {
        theStat.ATKBuff -= 50;
        StartBurstMode(_coeff);
    }

    private void EndBurstMode()
    {
        isBurstMode = false;
        theStat.ATKBuff -= 50;
        StartCoroutine(EndBurstReservation());
    }

    private IEnumerator EndBurstReservation()
    {
        yield return new WaitUntil(() => stateMachine.currentState != BurstAttackState && stateMachine.currentState != BurstAttackExitState);
        Animator.SetBool("Burst", false);
        UpdateGuage();
    }

    public void StartAtCombo3Manage()
    {
        StopCoroutine(nameof(StartAtCombo3Delay));
        if (startAtCombo3) StartCoroutine(StartAtCombo3Delay());
    }

    public IEnumerator StartAtCombo3Delay()
    {
        yield return new WaitForSeconds(0.2f);
        startAtCombo3 = false;
    }

    public override void SkillManage(int[] _result)
    {
        base.SkillManage(_result);
        switch (_result[0])
        {
            case 0:
                Skill01BeforeState.skill = GS.skillList[0];
                Skill01SuccessState.skill = GS.skillList[0];
                Skill01FailState.skill = GS.skillList[0];
                stateMachine.ChangeState(Skill01BeforeState);
                GraffitiHPHeal(_result[1]);
                break;
            case 1:
                if (isBurstMode)
                {
                    Skill02State.skill = GS.skillList[1];
                    stateMachine.ChangeState(Skill02State);
                }
                else
                {
                    stateMachine.ChangeState(IdleState);
                    dodgeGuage = 50;
                    attackGuage = 50;
                    UpdateGuage();
                }
                GraffitiHPHeal(_result[1]);
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
