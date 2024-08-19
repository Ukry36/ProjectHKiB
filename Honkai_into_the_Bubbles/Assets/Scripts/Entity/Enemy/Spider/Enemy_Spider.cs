using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Enemy_Spider : Enemy
{
    [SerializeField] private GameObject ExplosionPrefab;
    public Enemy_Spider_StateMachine StateMachine { get; private set; }
    public Enemy_Spider_IdleState IdleState { get; private set; }
    public Enemy_Spider_RandomIdleState RandomIdleState { get; private set; }
    public Enemy_Spider_RandomMoveState RandomMoveState { get; private set; }
    public Enemy_Spider_DirIdleState DirIdleState { get; private set; }
    public Enemy_Spider_DirMoveState DirMoveState { get; private set; }
    public Enemy_Spider_PathfindIdleState PFIdleState { get; private set; }
    public Enemy_Spider_PathfindMoveState PFMoveState { get; private set; }
    public Enemy_Spider_AggroMoveState AggroMoveState { get; private set; }
    public Enemy_Spider_KnockbackState KnockbackState { get; private set; }
    public Enemy_Spider_Skill01EnterState Skill01EnterState { get; private set; }
    public Enemy_Spider_Skill01State Skill01State { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new Enemy_Spider_StateMachine();

        IdleState = new Enemy_Spider_IdleState(this, StateMachine, "Idle");
        RandomIdleState = new Enemy_Spider_RandomIdleState(this, StateMachine, "Idle");
        RandomMoveState = new Enemy_Spider_RandomMoveState(this, StateMachine, "Walk");
        DirIdleState = new Enemy_Spider_DirIdleState(this, StateMachine, "Idle");
        DirMoveState = new Enemy_Spider_DirMoveState(this, StateMachine, "Walk");
        PFIdleState = new Enemy_Spider_PathfindIdleState(this, StateMachine, "Idle");
        PFMoveState = new Enemy_Spider_PathfindMoveState(this, StateMachine, "Walk");
        AggroMoveState = new Enemy_Spider_AggroMoveState(this, StateMachine, "Walk");
        KnockbackState = new Enemy_Spider_KnockbackState(this, StateMachine, "Knockback");
        Skill01EnterState = new Enemy_Spider_Skill01EnterState(this, StateMachine, "Skill01Enter");
        Skill01State = new Enemy_Spider_Skill01State(this, StateMachine, "Skill01");
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

    public override void Die()
    {
        PoolManager.instance.ReuseGameObject(ExplosionPrefab, this.transform.position, Quaternion.identity).GetComponent<Bullet>().theStat = theStat;
        base.Die();
    }

}
