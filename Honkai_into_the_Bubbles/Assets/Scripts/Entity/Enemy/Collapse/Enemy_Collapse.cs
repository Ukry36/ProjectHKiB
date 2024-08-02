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

        Debug.Log(StateMachine.currentState.GetType().Name);

    }

    /*

    만들어야 할 것:
    스킬우선도1: 
    skill01과 동일, 대신 hp가 50%미만인 경우 발동함.

    스킬우선도2: 
    skill01: 플레이어 위치로 이동하여 본인 자리에  2x2칸 대미지 (area detect)
    dmg = 40, strong = 3, cooltime = 5, delay = 0.2, detect radius = 4, animplaytime = 0.5

    스킬우선도3: 
    skill02: 전방 1칸(너비 2칸) 공격 (line detect with err = 1)
    dmg = 30, strong = 2, , cooltime = 1.5, delay = 0.2, detect radius = 2, animplaytime = 0.5

    표시되지 않은 나머지는 0으로 하시면 되는데 animplaytime같은 경우엔 공격준비 애니메이션 재생되는 시간보다 많게 하면 됩니다.

    

    설명:
    skill01 상세: 애니메이션의 공격 준비단계는 사라지는 부분까지 담고 이후 1초동안 사라져있는데 이 동안 공격위치에
    attack alert가 뜹니다. 이미 플레이어 위치까지 간 후이기 때문에 자기 자신의 위치에 띄우면 될 겁니다. 그리고 공격
    애니메이션은 다른 것들과 똑같은 방식으로 하면 됩니다.
    공격에 조건이 하나 더 붙는데, 기본적으로 플레이어 위치로 이동해서 공격하는 타입이라 플레이어 주변 
    3x3칸 내에 착지할 공간이 있어야 공격 준비단계에 들어갈 수 있습니다. (대신 플레이어는 벽으로 취급하지 않습니다.)
    

    skill02 상세: rusher의 skill02를 보고 같은 방식으로 만들면 됩니다. 애니메이션에 관해선 똑같이 하시면 되고
    대신 Track같은 경우는 track radius가 0이기에 필요 없습니다. 그리고 linedetect를 사용하면 공격준비에서
    이미 플레이어를 바라보는 상태이기 때문에 SetAnimDir도 없어도 됩니다.


    가장 일반적인 공격이 rusher의 skill02이기 때문에 이걸 참고하는 것이 가장 도움될 거에요.
    
*/
}
