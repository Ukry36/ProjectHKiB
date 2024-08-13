using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* skill01 상세: 애니메이션의 공격 준비단계는 사라지는 부분까지 담고 이후 1초동안 사라져있는데 이 동안 공격위치에
    attack alert가 뜹니다. 이미 플레이어 위치까지 간 후이기 때문에 자기 자신의 위치에 띄우면 될 겁니다. 그리고 공격
    애니메이션은 다른 것들과 똑같은 방식으로 하면 됩니다.
    공격에 조건이 하나 더 붙는데, 기본적으로 플레이어 위치로 이동해서 공격하는 타입이라 플레이어 주변 
    3x3칸 내에 착지할 공간이 있어야 공격 준비단계에 들어갈 수 있습니다. (대신 플레이어는 벽으로 취급하지 않습니다.) */

public class Enemy_Collapse_Skill01After : Enemy_Collapse_State
{
    public Enemy_Collapse_Skill01After(Enemy_Collapse _enemy, Enemy_Collapse_StateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            enemy.StartCoroutine(enemy.SkillCooltime(enemy.SkillArray[0]));
            stateMachine.ChangeState(enemy.AggroMoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}