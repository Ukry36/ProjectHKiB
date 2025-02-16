using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Enemy_Rusher : Enemy
{
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
        IdleState = new Enemy_Rusher_IdleState(this, stateMachine, "Idle", this);
        RandomIdleState = new Enemy_Rusher_RandomIdleState(this, stateMachine, "Idle", this);
        RandomMoveState = new Enemy_Rusher_RandomMoveState(this, stateMachine, "Walk", this);
        DirIdleState = new Enemy_Rusher_DirIdleState(this, stateMachine, "Idle", this);
        DirMoveState = new Enemy_Rusher_DirMoveState(this, stateMachine, "Walk", this);
        PFIdleState = new Enemy_Rusher_PathfindIdleState(this, stateMachine, "Idle", this);
        PFMoveState = new Enemy_Rusher_PathfindMoveState(this, stateMachine, "Walk", this);
        AggroMoveState = new Enemy_Rusher_AggroMoveState(this, stateMachine, "Walk", this);
        KnockbackState = new Enemy_Rusher_KnockbackState(this, stateMachine, "Knockback", this);
        Skill01EnterState = new Enemy_Rusher_Skill01EnterState(this, stateMachine, "Skill01Enter", this);
        Skill01State = new Enemy_Rusher_Skill01State(this, stateMachine, "Skill01", this);
        Skill02EnterState = new Enemy_Rusher_Skill02EnterState(this, stateMachine, "Skill02Enter", this);
        Skill02State = new Enemy_Rusher_Skill02State(this, stateMachine, "Skill02", this);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        stateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

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

    public override void AnimationFinishTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

}
