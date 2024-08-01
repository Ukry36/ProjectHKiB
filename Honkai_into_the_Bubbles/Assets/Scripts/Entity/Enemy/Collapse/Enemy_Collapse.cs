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
    1. Enemy_Collapse의 statemachine 및 전체 코드
    2. Enemy_Collapse의 게임오브젝트 구성.
    3. Enemy_Collapse의 animation 및 animator (sprite library는 만들어뒀습니다.)

    설명:
    1. 다른 몹들의 코드를 보면 공통적으로 DirIdle, DirMove, PathfindIdle, PathfindMove는 공통적으로 가지고 있고
        AggroIdle과 AggroMove중 AggroMove만 있는 경우가 있습니다. 이건 어그로 상태에선 어차피 계속 플레이어를 향해
        움직일 거라서 그런 건데 원거리공격을 하는 경우에는 가만히 있을 때도 있습니다. 그럴 때만 AggroIdle을 써요.
        그래서 이 친구도 둘 중에 AggroMove만 있으면 됩니다. 

        */
    /*
    다른 AggroMove 코드들을 보면 Lightning을 제외하면 모두 Pathfind를 통해 길을 찾고 한 발자국 움직이는 방식을
    취하고 있습니다. 그런데 이 친구들은 모두 1x1크기이기 때문에 사용할 수 있었던 거라 2x2짜리 몹의 pathfinding을
    하기 위해선 2x2크기의 몹에 적합한 방식으로 pathfinder를 써야 합니다. pathfinder의 로직은 한 칸의 중앙 위치마다 
    overlapcircle을 하여 그 칸에 벽이 있는지 확인한 후 길을 정하는 것인데 2x2짜리 이 친구는 한 칸의 중앙이 아닌
    칸과 칸 사이의 중앙을 보아야 합니다 격자판의 네모난 가운데가 아니라 선과 선이 만나는 곳을 확인하는 거죠. 
    그리고 그 벽에 대한 단서가 주어지면 그걸 토대로 경로를 계산해주는데, 마침 2x2몹들은 4칸 중 한 칸이라도 벽이면
    지나갈 수 없는 식이기 때문에 기존 pathfinder와 동일한 로직을 쓸 수 있습니다. 다시 말하면 같은 pathfinder를 
    사용해도 상관은 없습니다. 사실 정수기반으로 작동하는 pathfinder를 실수기반으로 작동하게 수정하면 되는 건데 
    이건 제가 해뒀어요. 궁금하시다면 A*알고리즘으로 한 번 찾아보시면 됩니다.
    *//*

        결론적으론 아마 Enemy_Spider의 AggroMoveState를 그대로 갖다 써도 괜찮을 것 같아요. Enemy_Rusher는 backstep
        과정이 같이 들어있을 거라서 약간 지워줘야 합니다.

        그리고 이제 공격 발동조건 부분이 if else문으로 쭉 있는데 이쪽은 자세히 읽어보면 이해하는데 큰 문제 없을 거에요.

        보다 보면 gazepoint, gazepointToDir4, 8 이런 것들이 있는데 이런 것들은 Gazepoint를 먼저 목표 위치에 두고
        gazepointToDir()을 불러주면 0, 1로만 이뤄진 Vector2를 반환합니다. 목표 위치의 방향으로 이동할 때도 사용할 
        수 있고 목표 위치를 바라보기만 할 때도 사용할 수 있어요. 

    2. 게임오브젝트의 구성같은 경우엔 플레이어와 비슷한데 그냥 attack이 없고 skill만 있을 겁니다. 다른 Enemy들의 오브젝트를
        참고해서 하시면 됩니다.

        3. 플레이어를 할 때와 같은 식으로 하면 됩니다. 
*/
}
