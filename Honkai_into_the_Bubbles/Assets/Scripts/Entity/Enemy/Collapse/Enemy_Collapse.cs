using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Collapse : Enemy
{
    public Enemy_Collapse_DirMoveState DirMoveState { get; private set; }
    public Enemy_Collapse_DirIdleState DirIdleState { get; private set; }
    public Enemy_Collapse_IdleState IdleState { get; private set; }
    public Enemy_Collapse_AggroMoveState AggroMoveState { get; private set; }
    public Enemy_Collapse_RandomIdleState RandomIdleState { get; private set; }
    public Enemy_Collapse_RandomMoveState RandomMoveState { get; private set; }
    public Enemy_Collapse_PathfindIdleState PFIdleState { get; private set; }
    public Enemy_Collapse_PathfindMoveState PFMoveState { get; private set; }
    public Enemy_Collapse_KnockbackState KnockbackState { get; private set; }
    public Enemy_Collapse_Skill01Before Skill01BeforeState { get; private set; }
    public Enemy_Collapse_Skill01Ing Skill01IngState { get; private set; }
    public Enemy_Collapse_Skill01After Skill01AfterState { get; private set; }
    public Enemy_Collapse_Skill02Enter Skill02EnterState { get; private set; }
    public Enemy_Collapse_Skill02 Skill02State { get; private set; }
    public BoxCollider2D EnemyWallBoxCollider;
    [HideInInspector] public bool canSkill01Passive;

    protected override void Awake()
    {
        base.Awake();
        IdleState = new Enemy_Collapse_IdleState(this, stateMachine, "Idle", this);
        RandomIdleState = new Enemy_Collapse_RandomIdleState(this, stateMachine, "Idle", this);
        RandomMoveState = new Enemy_Collapse_RandomMoveState(this, stateMachine, "Walk", this);
        DirIdleState = new Enemy_Collapse_DirIdleState(this, stateMachine, "Idle", this);
        DirMoveState = new Enemy_Collapse_DirMoveState(this, stateMachine, "Walk", this);
        PFIdleState = new Enemy_Collapse_PathfindIdleState(this, stateMachine, "Idle", this);
        PFMoveState = new Enemy_Collapse_PathfindMoveState(this, stateMachine, "Walk", this);
        AggroMoveState = new Enemy_Collapse_AggroMoveState(this, stateMachine, "Walk", this);
        Skill01BeforeState = new Enemy_Collapse_Skill01Before(this, stateMachine, "skill01Before", this);
        Skill01IngState = new Enemy_Collapse_Skill01Ing(this, stateMachine, "skill01Ing", this);
        Skill01AfterState = new Enemy_Collapse_Skill01After(this, stateMachine, "skill01After", this);
        Skill02EnterState = new Enemy_Collapse_Skill02Enter(this, stateMachine, "skill02Enter", this);
        Skill02State = new Enemy_Collapse_Skill02(this, stateMachine, "skill02", this);
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
    }

    public IEnumerator SkillCooltime(Skill skill)
    {
        skill.isCooltime = true;
        yield return new WaitForSeconds(skill.Cooltime);
        skill.isCooltime = false;
    }

    public override void AnimationFinishTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
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
}