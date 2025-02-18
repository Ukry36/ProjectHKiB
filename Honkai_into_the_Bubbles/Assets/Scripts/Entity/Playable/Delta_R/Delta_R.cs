using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Delta_R : Playable
{
    public Delta_R_IdleState IdleState { get; private set; }
    public Delta_R_WalkState WalkState { get; private set; }
    public Delta_R_DodgeState DodgeState { get; private set; }
    public Delta_R_KeepDodgeState KeepDodgeState { get; private set; }
    public Delta_R_GraffitiEnterState GraffitiEnterState { get; private set; }
    public Delta_R_GraffitiIngState GraffitiState { get; private set; }
    public Delta_R_GraffitiExitState GraffitiExitState { get; private set; }
    public Delta_R_KnockbackState KnockbackState { get; private set; }
    public Delta_R_BasicAttackState AttackState { get; private set; }
    public Delta_R_BasicAttackExitState AttackExitState { get; private set; }
    public Delta_R_SkillState SkillState { get; private set; }


    public bool skill01ing = false;
    public bool skill02ing = false;

    public int repeatSkill03;
    public GameObject SAPrefabfor0301;
    public GameObject SAPrefabfor0301Diag;
    public GameObject SAPrefabfor03only;
    public GameObject SAPrefabfor03onlyDiag;
    public GameObject AttractorPrefab;


    protected override void Awake()
    {
        base.Awake();
        IdleState = new Delta_R_IdleState(this, stateMachine, "Idle", this);
        WalkState = new Delta_R_WalkState(this, stateMachine, "Walk", this);
        DodgeState = new Delta_R_DodgeState(this, stateMachine, "DodgeIng", this);
        KeepDodgeState = new Delta_R_KeepDodgeState(this, stateMachine, "DodgeIng", this);
        GraffitiEnterState = new Delta_R_GraffitiEnterState(this, stateMachine, "DodgeEnter", this);
        GraffitiState = new Delta_R_GraffitiIngState(this, stateMachine, "DodgeIng", this);
        GraffitiExitState = new Delta_R_GraffitiExitState(this, stateMachine, "DodgeExit", this);
        KnockbackState = new Delta_R_KnockbackState(this, stateMachine, "Knockback", this);
        AttackState = new Delta_R_BasicAttackState(this, stateMachine, "Attack", this);
        AttackExitState = new Delta_R_BasicAttackExitState(this, stateMachine, "AttackExit", this);
        SkillState = new Delta_R_SkillState(this, stateMachine, "Skill01", this);
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

    public override void ChangeSpriteLibraryAsset()
    {
        base.ChangeSpriteLibraryAsset();

        SAPrefabfor0301.GetComponent<SpriteRenderer>().color = PlayerManager.instance.ThemeColors[0];
        SAPrefabfor0301Diag.GetComponent<SpriteRenderer>().color = PlayerManager.instance.ThemeColors[1];
        SAPrefabfor03only.GetComponent<SpriteRenderer>().color = PlayerManager.instance.ThemeColors[0];
        SAPrefabfor03onlyDiag.GetComponent<SpriteRenderer>().color = PlayerManager.instance.ThemeColors[1];
    }

    public override void SkillManage(int[] _result)
    {
        base.SkillManage(_result);
        switch (_result[0])
        {
            case 0:
                skill01ing = true;
                SkillState.skill = GS.skillList[0];
                stateMachine.ChangeState(SkillState);
                GraffitiHPHeal(_result[1]);
                break;
            case 1:
                theStat.GPControl(GS.skillList[1].GraffitiPoint);
                if (skill01ing || skill02ing)
                {
                    skill02ing = true;
                    skill01ing = false;
                    theStat.ATKBuff += 50;
                    SkillState.skill = GS.skillList[1];
                    stateMachine.ChangeState(SkillState);
                    break;
                }
                stateMachine.ChangeState(IdleState);
                GraffitiHPHeal(_result[1]);
                break;
            case 2:
                if (skill01ing)
                {
                    SkillState.skill = GS.skillList[0];
                    StartCoroutine(Skill03p01Coroutine(GS.skillList[2]));
                    stateMachine.ChangeState(SkillState);
                }
                else if (skill02ing)
                {
                    SkillState.skill = GS.skillList[1];
                    StartCoroutine(Skill03p01Coroutine(GS.skillList[2]));
                    stateMachine.ChangeState(SkillState);
                }
                else
                {
                    AttackState.combo = 2;
                    stateMachine.ChangeState(AttackState);
                    StartCoroutine(Skill03Coroutine(GS.skillList[2]));
                }
                GraffitiHPHeal(_result[1]);
                break;

            default:
                skill01ing = false;
                stateMachine.ChangeState(IdleState);
                break;
        }
    }

    private int RSACI = 0;

    private IEnumerator Skill03p01Coroutine(Skill _skill)
    {
        float prevTime = 0;
        float lastTime = 0;
        while (skill01ing || skill02ing)
        {
            lastTime += Time.deltaTime;
            yield return null;
            if (prevTime + _skill.Delay * (skill02ing ? 0.5 : 1) < lastTime)
            {
                FireSA03p01();
                prevTime = lastTime;
            }
        }
    }

    private void FireSA03p01()
    {
        if (RSACI % 2 == 0)
        {
            PoolManager.instance.ReuseGameObject(SAPrefabfor0301, this.transform.position, Quaternion.identity).GetComponent<SpriteRenderer>().color = PlayerManager.instance.ThemeColors[0];
            PoolManager.instance.ReuseGameObject(SAPrefabfor0301, this.transform.position, Quaternion.Euler(0, 0, 90)).GetComponent<SpriteRenderer>().color = PlayerManager.instance.ThemeColors[0];
            PoolManager.instance.ReuseGameObject(SAPrefabfor0301, this.transform.position, Quaternion.Euler(0, 0, 180)).GetComponent<SpriteRenderer>().color = PlayerManager.instance.ThemeColors[0];
            PoolManager.instance.ReuseGameObject(SAPrefabfor0301, this.transform.position, Quaternion.Euler(0, 0, 270)).GetComponent<SpriteRenderer>().color = PlayerManager.instance.ThemeColors[0];
        }
        else
        {
            PoolManager.instance.ReuseGameObject(SAPrefabfor0301Diag, this.transform.position, Quaternion.Euler(0, 0, 45)).GetComponent<SpriteRenderer>().color = PlayerManager.instance.ThemeColors[1];
            PoolManager.instance.ReuseGameObject(SAPrefabfor0301Diag, this.transform.position, Quaternion.Euler(0, 0, 135)).GetComponent<SpriteRenderer>().color = PlayerManager.instance.ThemeColors[1];
            PoolManager.instance.ReuseGameObject(SAPrefabfor0301Diag, this.transform.position, Quaternion.Euler(0, 0, 225)).GetComponent<SpriteRenderer>().color = PlayerManager.instance.ThemeColors[1];
            PoolManager.instance.ReuseGameObject(SAPrefabfor0301Diag, this.transform.position, Quaternion.Euler(0, 0, 315)).GetComponent<SpriteRenderer>().color = PlayerManager.instance.ThemeColors[1];
        }
        RSACI++;
    }

    private IEnumerator Skill03Coroutine(Skill _skill)
    {
        for (int i = 0; i < repeatSkill03; i++)
        {
            yield return new WaitForSeconds(0.4f / repeatSkill03);
            FireSA03();
        }
    }


    private void FireSA03()
    {
        if (RSACI % 2 == 0)
        {
            PoolManager.instance.ReuseGameObject(SAPrefabfor03only, this.transform.position, Quaternion.LookRotation(forward: Vector3.forward, upwards: moveDir)).GetComponent<SpriteRenderer>().color = PlayerManager.instance.ThemeColors[0];
        }
        else
        {
            PoolManager.instance.ReuseGameObject(SAPrefabfor03onlyDiag, this.transform.position, Quaternion.LookRotation(forward: Vector3.forward, upwards: moveDir)).GetComponent<SpriteRenderer>().color = PlayerManager.instance.ThemeColors[1];
        }
        RSACI++;
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

}
