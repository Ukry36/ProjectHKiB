using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
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

    public GameObject[] Bullet01;
    public GameObject Bullet02;
    public GameObject targetPrefab;

    private readonly Vector3[][] Ofsets = new Vector3[][]
    {
        new Vector3[]
        {
            new(1.2f, 0.9f, 0), new(-1.2f, 0.9f, 0),
            new(0.8f, 0.95f, 0), new(-0.8f, 0.95f, 0),
            new(1f, 0.35f, 0), new(-1f, 0.35f, 0)
        },
        new Vector3[]
        {
            new(0, 1.9f, 0), new(0, -0.1f, 0),
            new(0, 1.6f, 0), new(0, 0.4f, 0),
            new(0, 1.6f, 0), new(0, -0.2f, 0)
        },
        new Vector3[]
        {
            new(1.2f, 1.4f, 0), new(-1.2f, 1.4f, 0),
            new(0.8f, 1.25f, 0), new(-0.8f, 1.25f, 0),
            new(1f, 1f, 0), new(-1f, 1f, 0)
        },
        new Vector3[]
        {
            new(0, 1.9f, 0), new(0, -0.1f, 0),
            new(0, 1.6f, 0), new(0, 0.4f, 0),
            new(0, 1.6f, 0), new(0, -0.2f, 0)
        }
    };

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

    public void ShootBullet01(Vector2 _dir)
    {
        if (_dir.y < 0) // D
        {
            PoolManager.instance.ReuseGameObject(Bullet01[0], transform.position, Quaternion.identity)
            .GetComponent<Bullet>().theStat = theStat;

            PoolManager.instance.ReuseGameObject(Bullet01[1], transform.position, Quaternion.identity)
            .GetComponent<Bullet>().theStat = theStat;
        }
        else if (_dir.x > 0) // R
        {
            PoolManager.instance.ReuseGameObject(Bullet01[2], transform.position, Quaternion.identity)
            .GetComponent<Bullet>().theStat = theStat;
            PoolManager.instance.ReuseGameObject(Bullet01[3], transform.position, Quaternion.identity)
            .GetComponent<Bullet>().theStat = theStat;
        }
        else if (_dir.y > 0) // U
        {
            PoolManager.instance.ReuseGameObject(Bullet01[4], transform.position, Quaternion.identity)
            .GetComponent<Bullet>().theStat = theStat;
            PoolManager.instance.ReuseGameObject(Bullet01[5], transform.position, Quaternion.identity)
            .GetComponent<Bullet>().theStat = theStat;
        }
        else if (_dir.x < 0) // L
        {
            PoolManager.instance.ReuseGameObject(Bullet01[6], transform.position, Quaternion.identity)
            .GetComponent<Bullet>().theStat = theStat;
            PoolManager.instance.ReuseGameObject(Bullet01[7], transform.position, Quaternion.identity)
            .GetComponent<Bullet>().theStat = theStat;
        }
    }


    private void FireMissile(Vector2 _dir, int _muzzle)
    {
        quaternion dir = quaternion.identity;
        Vector3 ofs1 = Vector3.zero;
        Vector3 ofs2 = Vector3.zero;

        if (_dir.y < 0) // D
        {
            dir = Quaternion.Euler(0, 0, 180);
            ofs1 = Ofsets[0][_muzzle * 2];
            ofs2 = Ofsets[0][_muzzle * 2 + 1];
        }
        else if (_dir.x > 0) // R
        {
            dir = Quaternion.Euler(0, 0, -70);
            ofs1 = Ofsets[1][_muzzle * 2];
            ofs2 = Ofsets[1][_muzzle * 2 + 1];
        }
        else if (_dir.y > 0) // U
        {
            dir = Quaternion.identity;
            ofs1 = Ofsets[2][_muzzle * 2];
            ofs2 = Ofsets[2][_muzzle * 2 + 1];
        }
        else if (_dir.x < 0) // L
        {
            dir = Quaternion.Euler(0, 0, 70);
            ofs1 = Ofsets[3][_muzzle * 2];
            ofs2 = Ofsets[3][_muzzle * 2 + 1];
        }

        if (PoolManager.instance.ReuseGameObject(Bullet02, Mover.transform.position + ofs1, dir)
        .TryGetComponent(out Missile missile))
        {
            missile.targetPos = PoolManager.instance.ReuseGameObject(targetPrefab, GetRandomPos(GazePoint.transform.position, 5), Quaternion.identity).transform;
            missile.theStat = theStat;
        }

        if (PoolManager.instance.ReuseGameObject(Bullet02, Mover.transform.position + ofs2, dir)
        .TryGetComponent(out Missile missile1))
        {
            missile1.targetPos = PoolManager.instance.ReuseGameObject(targetPrefab, GetRandomPos(GazePoint.transform.position, 5), Quaternion.identity).transform;
            missile1.theStat = theStat;
        }
    }

    public IEnumerator ShootBullet02(Vector2 _dir)
    {
        for (int i = 0; i < 6; i++)
        {
            yield return new WaitForSeconds(0.2f);
            FireMissile(_dir, i % 3);
        }
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
