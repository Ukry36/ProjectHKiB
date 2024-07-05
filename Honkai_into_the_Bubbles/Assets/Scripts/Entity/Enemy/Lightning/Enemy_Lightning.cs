using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Enemy_Lightning : Enemy
{
    public Enemy_Lightning_StateMachine StateMachine { get; private set; }
    public Enemy_Lightning_IdleState IdleState { get; private set; }
    public Enemy_Lightning_RandomIdleState RandomIdleState { get; private set; }
    public Enemy_Lightning_RandomMoveState RandomMoveState { get; private set; }
    public Enemy_Lightning_DirIdleState DirIdleState { get; private set; }
    public Enemy_Lightning_DirMoveState DirMoveState { get; private set; }
    public Enemy_Lightning_PathfindIdleState PFIdleState { get; private set; }
    public Enemy_Lightning_PathfindMoveState PFMoveState { get; private set; }
    public Enemy_Lightning_AggroIdleState AggroIdleState { get; private set; }
    public Enemy_Lightning_AggroMoveState AggroMoveState { get; private set; }
    public Enemy_Lightning_KnockbackState KnockbackState { get; private set; }
    public Enemy_Lightning_Skill01EnterState Skill01EnterState { get; private set; }
    public Enemy_Lightning_Skill01State Skill01State { get; private set; }
    public Enemy_Lightning_Skill02EnterState Skill02EnterState { get; private set; }
    public Enemy_Lightning_Skill02State Skill02State { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new Enemy_Lightning_StateMachine();

        IdleState = new Enemy_Lightning_IdleState(this, StateMachine, "Idle");
        RandomIdleState = new Enemy_Lightning_RandomIdleState(this, StateMachine, "Idle");
        RandomMoveState = new Enemy_Lightning_RandomMoveState(this, StateMachine, "Walk");
        DirIdleState = new Enemy_Lightning_DirIdleState(this, StateMachine, "Idle");
        DirMoveState = new Enemy_Lightning_DirMoveState(this, StateMachine, "Walk");
        PFIdleState = new Enemy_Lightning_PathfindIdleState(this, StateMachine, "Idle");
        PFMoveState = new Enemy_Lightning_PathfindMoveState(this, StateMachine, "Walk");
        AggroIdleState = new Enemy_Lightning_AggroIdleState(this, StateMachine, "Idle");
        AggroMoveState = new Enemy_Lightning_AggroMoveState(this, StateMachine, "Walk");
        KnockbackState = new Enemy_Lightning_KnockbackState(this, StateMachine, "Knockback");
        Skill01EnterState = new Enemy_Lightning_Skill01EnterState(this, StateMachine, "Skill01Enter");
        Skill01State = new Enemy_Lightning_Skill01State(this, StateMachine, "Skill01");
        Skill02EnterState = new Enemy_Lightning_Skill02EnterState(this, StateMachine, "Skill02Enter");
        Skill02State = new Enemy_Lightning_Skill02State(this, StateMachine, "Skill02");
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

    public override bool MovepointAdjustCheck()
    {
        Vector3 DirX = new(moveDir.x, 0, 0);
        Vector3 DirY = new(0, moveDir.y, 0);

        if (moveDir.x == 0 || moveDir.y == 0)
        {
            if (Physics2D.OverlapCircle(MovePoint.transform.position + moveDir * 1.5f, .4f, wallLayer))
                return true;
        }
        else // moveInput.x != 0 && moveInput.y != 0
        {
            if (Physics2D.OverlapCircle(MovePoint.transform.position + DirX * 1.5f, .4f, wallLayer))
                moveDir.x = 0;

            if (Physics2D.OverlapCircle(MovePoint.transform.position + DirY * 1.5f, .4f, wallLayer))
                moveDir.y = 0;

            if (moveDir == Vector3.zero)
                return true;

            if (moveDir.x != 0 && moveDir.y != 0)
                if (Physics2D.OverlapCircle(MovePoint.transform.position + moveDir * 1.5f, .4f, wallLayer))
                    MovePoint.transform.position -= DirY;
        }
        return false;
    }

    public IEnumerator Skill01Cooltime()
    {
        SkillArray[0].isCooltime = true;
        yield return new WaitForSeconds(SkillArray[0].Cooltime);
        SkillArray[0].isCooltime = false;
    }

    public IEnumerator Skill02Cooltime()
    {
        SkillArray[1].isCooltime = true;
        yield return new WaitForSeconds(SkillArray[1].Cooltime);
        SkillArray[1].isCooltime = false;
    }

}
