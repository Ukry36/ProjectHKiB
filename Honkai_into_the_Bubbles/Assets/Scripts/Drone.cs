using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MoveViaAlgorithm
{
    private WaitForSeconds wait = new WaitForSeconds(0.5f);
    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        pathFinder = GetComponentInChildren<PathFindManager>();
        Debug.Log(pathFinder);
        Player = FindObjectOfType<PlayerManager>().transform;
        PlayerState = Player.GetComponent<State>();
        moveSpeed = defaultSpeed;

    }

    private void Update()
    {
        /////////////// 링크의 카메라 움직임 + 플레이어와 거리가 일정 이하면 움직이지 않도록

        ////////////// DetectEnemy가 true 따라가는 타겟을 해당 enemy로 바꿈

        ///////////// 타겟이 enemy이면 enemy의 방향으로 총알을 발사

        ///////////// 플레이어와 거리가 너무 멀면 enemy를 따라가는 것을 중지

    }
}
