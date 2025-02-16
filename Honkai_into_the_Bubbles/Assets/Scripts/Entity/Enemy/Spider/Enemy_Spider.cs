using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Enemy_Spider : Enemy
{
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

    [SerializeField] private GameObject ExplosionPrefab;

    protected override void Awake()
    {
        base.Awake();

        IdleState = new Enemy_Spider_IdleState(this, stateMachine, "Idle", this);
        RandomIdleState = new Enemy_Spider_RandomIdleState(this, stateMachine, "Idle", this);
        RandomMoveState = new Enemy_Spider_RandomMoveState(this, stateMachine, "Walk", this);
        DirIdleState = new Enemy_Spider_DirIdleState(this, stateMachine, "Idle", this);
        DirMoveState = new Enemy_Spider_DirMoveState(this, stateMachine, "Walk", this);
        PFIdleState = new Enemy_Spider_PathfindIdleState(this, stateMachine, "Idle", this);
        PFMoveState = new Enemy_Spider_PathfindMoveState(this, stateMachine, "Walk", this);
        AggroMoveState = new Enemy_Spider_AggroMoveState(this, stateMachine, "Walk", this);
        KnockbackState = new Enemy_Spider_KnockbackState(this, stateMachine, "Knockback", this);
        Skill01EnterState = new Enemy_Spider_Skill01EnterState(this, stateMachine, "Skill01Enter", this);
        Skill01State = new Enemy_Spider_Skill01State(this, stateMachine, "Skill01", this);
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

    public override void Die()
    {
        PoolManager.instance.ReuseGameObject(ExplosionPrefab, this.transform.position, Quaternion.identity).GetComponent<Bullet>().theStat = theStat;
        base.Die();
    }

    public override void AnimationFinishTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

}
