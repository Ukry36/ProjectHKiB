using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Collapse : Enemy
{
    public Enemy_Collapse_StateMachine StateMachine { get; private set; }

    public Enemy_Collapse_DirMoveState DirMoveState { get; private set; }
    public Enemy_Collapse_DirIdleState DirIdleState { get; private set; }
    public Enemy_Collapse_IdleState IdleState { get; private set; }
    public Enemy_Collapse_AggroMoveState AggroMoveState { get; private set; }
    public Enemy_Collapse_RandomIdleState RandomIdleState { get; private set; }
    public Enemy_Collapse_RandomMoveState RandomMoveState { get; private set; }
    public Enemy_Collapse_PathfindIdleState PFIdleState { get; private set; }
    public Enemy_Collapse_PathfindMoveState PFMoveState { get; private set; }

    public Enemy_Collapse_Skill01Before Skill01BeforeState { get; private set; }
    public Enemy_Collapse_Skill01Ing Skill01IngState { get; private set; }
    public Enemy_Collapse_Skill01After Skill01AfterState { get; private set; }
    public Enemy_Collapse_Skill02Enter Skill02EnterState { get; private set; }
    public Enemy_Collapse_Skill02 Skill02State { get; private set; }
    public BoxCollider2D EnemyWallBoxCollider;

    protected override void Awake()
    {
        base.Awake();

        StateMachine = new Enemy_Collapse_StateMachine();

        IdleState = new Enemy_Collapse_IdleState(this, StateMachine, "Idle");
        RandomIdleState = new Enemy_Collapse_RandomIdleState(this, StateMachine, "Idle");
        RandomMoveState = new Enemy_Collapse_RandomMoveState(this, StateMachine, "Walk");
        DirIdleState = new Enemy_Collapse_DirIdleState(this, StateMachine, "Idle");
        DirMoveState = new Enemy_Collapse_DirMoveState(this, StateMachine, "Walk");
        PFIdleState = new Enemy_Collapse_PathfindIdleState(this, StateMachine, "Idle");
        PFMoveState = new Enemy_Collapse_PathfindMoveState(this, StateMachine, "Walk");
        AggroMoveState = new Enemy_Collapse_AggroMoveState(this, StateMachine, "Walk");
        Skill01BeforeState = new Enemy_Collapse_Skill01Before(this, StateMachine, "skill01Before");
        Skill01IngState = new Enemy_Collapse_Skill01Ing(this, StateMachine, "skill01Ing");
        Skill01AfterState = new Enemy_Collapse_Skill01After(this, StateMachine, "skill01After");
        Skill02EnterState = new Enemy_Collapse_Skill02Enter(this, StateMachine, "skill02Enter");
        Skill02State = new Enemy_Collapse_Skill02(this, StateMachine, "skill02");
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

    public IEnumerator SkillCooltime(Skill skill)
    {
        skill.isCooltime = true;
        yield return new WaitForSeconds(skill.Cooltime);
        skill.isCooltime = false;
    }
}