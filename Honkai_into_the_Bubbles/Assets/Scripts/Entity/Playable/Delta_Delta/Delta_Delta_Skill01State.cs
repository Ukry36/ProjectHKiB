using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta_Delta_Skill01State : Playable_State
{
    public Skill skill;
    private Delta_Delta player;
    public Delta_Delta_Skill01State(Playable _playerBase, Playable_StateMachine _stateMachine, string _animBoolName, Delta_Delta _player) : base(_playerBase, _stateMachine, _animBoolName)
    {
        this.player = _player;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
