using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Enemy_Rusher : Enemy
{
    public Enemy_Rusher_StateMachine StateMachine { get; private set; }
    public Enemy_Rusher_IdleState IdleState { get; private set; }
    public Enemy_Rusher_RandomIdleState RandomIdleState { get; private set; }
    public Enemy_Rusher_RandomMoveState RandomMoveState { get; private set; }
    public Enemy_Rusher_DirIdleState DirIdleState { get; private set; }
    public Enemy_Rusher_DirMoveState DirMoveState { get; private set; }
    public Enemy_Rusher_PathfindIdleState PFIdleState { get; private set; }
    public Enemy_Rusher_PathfindMoveState PFMoveState { get; private set; }
    public Enemy_Rusher_AggroMoveState AggroMoveState { get; private set; }
    public Enemy_Rusher_KnockbackState KnockbackState { get; private set; }
    public Enemy_Rusher_Skill01EnterState Skill01EnterState { get; private set; }
    public Enemy_Rusher_Skill01State Skill01State { get; private set; }
    public Enemy_Rusher_Skill02EnterState Skill02EnterState { get; private set; }
    public Enemy_Rusher_Skill02State Skill02State { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new Enemy_Rusher_StateMachine();

        IdleState = new Enemy_Rusher_IdleState(this, StateMachine, "Idle");
        RandomIdleState = new Enemy_Rusher_RandomIdleState(this, StateMachine, "Idle");
        RandomMoveState = new Enemy_Rusher_RandomMoveState(this, StateMachine, "Walk");
        DirIdleState = new Enemy_Rusher_DirIdleState(this, StateMachine, "Idle");
        DirMoveState = new Enemy_Rusher_DirMoveState(this, StateMachine, "Walk");
        PFIdleState = new Enemy_Rusher_PathfindIdleState(this, StateMachine, "Idle");
        PFMoveState = new Enemy_Rusher_PathfindMoveState(this, StateMachine, "Walk");
        AggroMoveState = new Enemy_Rusher_AggroMoveState(this, StateMachine, "Walk");
        KnockbackState = new Enemy_Rusher_KnockbackState(this, StateMachine, "Knockback");
        Skill01EnterState = new Enemy_Rusher_Skill01EnterState(this, StateMachine, "Skill01Enter");
        Skill01State = new Enemy_Rusher_Skill01State(this, StateMachine, "Skill01");
        Skill02EnterState = new Enemy_Rusher_Skill02EnterState(this, StateMachine, "Skill02Enter");
        Skill02State = new Enemy_Rusher_Skill02State(this, StateMachine, "Skill02");
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
