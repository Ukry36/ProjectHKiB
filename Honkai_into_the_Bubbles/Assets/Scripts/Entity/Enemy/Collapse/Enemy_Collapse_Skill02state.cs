using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* skill02 상세: rusher의 skill02를 보고 같은 방식으로 만들면 됩니다. 애니메이션에 관해선 똑같이 하시면 되고
    대신 Track같은 경우는 track radius가 0이기에 필요 없습니다. 그리고 linedetect를 사용하면 공격준비에서
    이미 플레이어를 바라보는 상태이기 때문에 SetAnimDir도 없어도 됩니다. */

public class Enemy_Collapse_Skill02 : Enemy_Collapse_State
{


    public Enemy_Collapse_Skill02(Enemy_Collapse _enemy, Enemy_Collapse_StateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
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
            enemy.StartCoroutine(enemy.SkillCooltime(enemy.SkillArray[1]));
            enemy.StateMachine.ChangeState(enemy.AggroMoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}