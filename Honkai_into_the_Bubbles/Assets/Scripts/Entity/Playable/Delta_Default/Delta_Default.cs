using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Default : Playable
{
    public Delta_Default_StateMachine StateMachine { get; private set; }
    public Delta_Default_IdleState IdleState { get; private set; }
    public Delta_Default_WalkState WalkState { get; private set; }
    public Delta_Default_DodgeEnterState DodgeEnterState { get; private set; }
    public Delta_Default_DodgeIngState DodgeState { get; private set; }
    public Delta_Default_DodgeExitState DodgeExitState { get; private set; }
    public Delta_Default_KnockbackState KnockbackState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new Delta_Default_StateMachine();

        IdleState = new Delta_Default_IdleState(this, StateMachine, "Idle");
        WalkState = new Delta_Default_WalkState(this, StateMachine, "Walk");
        DodgeEnterState = new Delta_Default_DodgeEnterState(this, StateMachine, "DodgeEnter");
        DodgeState = new Delta_Default_DodgeIngState(this, StateMachine, "DodgeIng");
        DodgeExitState = new Delta_Default_DodgeExitState(this, StateMachine, "DodgeExit");
        KnockbackState = new Delta_Default_KnockbackState(this, StateMachine, "Knockback");
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
    }

    protected virtual void Update()
    {
        StateMachine.currentState.Update();
        if (InputManager.instance.DodgeInput && StateMachine.currentState != DodgeState)
            if (canDodgeEffect || PlayerManager.instance.forcedCanDodge)
                StateMachine.ChangeState(DodgeEnterState);
    }

    public override void Knockback(Vector2 _dir, int _strong)
    {
        base.Knockback(_dir, _strong);

        int Coeff = (int)(_strong - Mass);

        if (!theStat.superArmor && Coeff > 0)
        {
            StateMachine.ChangeState(KnockbackState);
            KnockbackState.dir = _dir;
            KnockbackState.coeff = Coeff;
        }
    }
}
