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

    public override void SkillManage(int _skillNum)
    {
        base.SkillManage(_skillNum);

        // 이전 스크립트 (Player_R.cs)에 있는 걸 각 skill01, 02state에 옮기고
        // 어떨 때 발동하는지를 여기서 결정 

        // _skillnum = 0 : skill01, 1 : skill02 ...  -1 : fail

        // if (StateMachine.currentState == Skill01State) ~~ (X) (graffiti가 끝나면 idlestate로 돌아오기에 사용할 수 없음)
        // 떄문에 다른 방식으로 (ex: 새로운 bool 변수로 조절) 확인해야함

        // 회의중엔 playable 스크립트에 skillArray를 만들었었는데 그렇게 할 것이 아니라 
        // GraffitiSystem(GS) 에 있는 skillList를 사용할 것입니다.
        // skill정보는 GS.skillList[~~] 하는 식으로 받으면 됩니다.



        // ----- skill 정보 ----- //

        // skill01 : 일반 회오리공격 
        // animator에서 회오리공격 하는 animation 재생
        // 해당 애니메이션에서 skill01오브젝트를 activate/deactivate하며 대미지 줌
        // skill01state에선 statetimer를 지속 시간으로 설정해서 지속시간이 끝나면 (statetimer<0)
        // player.StateMachine.ChangeState(player.IdleState)
        // 만약 지속시간이 끝나기 전에 InputManager.instance.AttackInput을 받으면 기본공격 4격으로 이동 

        // skill02 : skill01중 발동시 
        // theStat.GPControl(GS.skillList[1].GraffitiPoint);
        // 그리고 skill01 지속시간 초기화 및 지속시간 증가 
        // skill01이 아닐 땐 GPControl만 일어남
        // (skill02state로 굳이 전환할 필요 없이 여기서 처리하면 좋을 것 같습니다.)
        // (결론적으로 skill02State 스크립트는 사용하지 않을 것 같으니 삭제해도 좋을 듯 합니다.)

        // skill03 : 일반적으론 combo3의 일반공격이 나오며 그 마지막에 칼날 프리팹 날리기
        // (AttackState에서 처리해도 될 것 같습니다)
        // skill01중 발동시 8방으로 날리는 것이 지속.
        // (skill01state에서 같이 처리하면 편할 듯 합니다)


        // skill마다 커맨드(바닥에 그리는 모양)이 있는데 그건 엑셀 파일 말고 영상을 참고해서 해주세요
        // 스킬 커맨드 설정은 R오브젝트에 붙어있는 GraffitiSystem에 있습니다.

        // 중요한 건 Player_R.cs에 이미 코루틴으로 구현된 걸 각 스킬의 state나 여기로 옮기는 것!!
        // 영상에선 어떻게 작동하는지 확인하시고 코드는 Player_R.cs를 참고하시면 될 것 같습니다!

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
