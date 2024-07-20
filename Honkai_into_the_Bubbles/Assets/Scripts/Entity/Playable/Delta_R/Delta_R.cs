using System;
using System.Collections;
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
        SkillState = new Delta_R_SkillState(this, StateMachine, "Skill01");
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
                if (!Physics2D.OverlapCircle(MovePoint.transform.position, .4f, LayerManager.instance.graffitiWallLayer))
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

    public override void ChangeSpriteLibraryAsset()
    {
        base.ChangeSpriteLibraryAsset();

        SAPrefabfor0301.GetComponent<SpriteRenderer>().color = PlayerManager.instance.ThemeColors[0];
        SAPrefabfor0301Diag.GetComponent<SpriteRenderer>().color = PlayerManager.instance.ThemeColors[1];
        SAPrefabfor03only.GetComponent<SpriteRenderer>().color = PlayerManager.instance.ThemeColors[0];
        SAPrefabfor03onlyDiag.GetComponent<SpriteRenderer>().color = PlayerManager.instance.ThemeColors[1];
    }

    public override void SkillManage(int _skillNum)
    {
        base.SkillManage(_skillNum);

        switch (_skillNum)
        {
            case 0:
                skill01ing = true;
                SkillState.skill = GS.skillList[0];
                StateMachine.ChangeState(SkillState);

                break;
            case 1:
                theStat.GPControl(GS.skillList[1].GraffitiPoint);
                if (skill01ing || skill02ing)
                {
                    skill02ing = true;
                    skill01ing = false;
                    SkillState.skill = GS.skillList[1];
                    StateMachine.ChangeState(SkillState);
                    break;
                }
                StateMachine.ChangeState(IdleState);
                break;
            case 2:
                if (skill01ing)
                {
                    SkillState.skill = GS.skillList[0];
                    StartCoroutine(Skill03p01Coroutine(GS.skillList[2]));
                    StateMachine.ChangeState(SkillState);
                }
                else if (skill02ing)
                {
                    SkillState.skill = GS.skillList[1];
                    StartCoroutine(Skill03p01Coroutine(GS.skillList[2]));
                    StateMachine.ChangeState(SkillState);
                }
                else
                {
                    AttackState.combo = 2;
                    StateMachine.ChangeState(AttackState);
                    StartCoroutine(Skill03Coroutine(GS.skillList[2]));
                }
                break;

            default:
                skill01ing = false;
                StateMachine.ChangeState(IdleState);
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
            if (prevTime + _skill.Delay < lastTime)
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
            var clone = Instantiate(SAPrefabfor0301, this.transform.position, Quaternion.identity);
            clone.SetActive(true);
            clone = Instantiate(SAPrefabfor0301, this.transform.position, Quaternion.Euler(0, 0, 90));
            clone.SetActive(true);
            clone = Instantiate(SAPrefabfor0301, this.transform.position, Quaternion.Euler(0, 0, 180));
            clone.SetActive(true);
            clone = Instantiate(SAPrefabfor0301, this.transform.position, Quaternion.Euler(0, 0, 270));
            clone.SetActive(true);
        }
        else
        {
            var clone = Instantiate(SAPrefabfor0301Diag, this.transform.position, Quaternion.Euler(0, 0, 45));
            clone.SetActive(true);
            clone = Instantiate(SAPrefabfor0301Diag, this.transform.position, Quaternion.Euler(0, 0, 135));
            clone.SetActive(true);
            clone = Instantiate(SAPrefabfor0301Diag, this.transform.position, Quaternion.Euler(0, 0, 225));
            clone.SetActive(true);
            clone = Instantiate(SAPrefabfor0301Diag, this.transform.position, Quaternion.Euler(0, 0, 315));
            clone.SetActive(true);
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
            //Vector3 quaternionToTarget = Quaternion.Euler(0, 0, this.transform.rotation.z) * applyVector;
            var clone = Instantiate(SAPrefabfor03only, this.transform.position, Quaternion.LookRotation(forward: Vector3.forward, upwards: savedInput));
            clone.SetActive(true);
        }
        else
        {
            var clone = Instantiate(SAPrefabfor03onlyDiag, this.transform.position, Quaternion.LookRotation(forward: Vector3.forward, upwards: savedInput));
            clone.SetActive(true);
        }
        RSACI++;
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
