using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Node
{
    public Node(bool _isWall, float _x, float _y, int _pfX, int _pfY)
    {
        isWall = _isWall;
        x = _x;
        y = _y;
        pfX = _pfX;
        pfY = _pfY;
    }

    public bool isWall;
    public Node ParentNode;

    // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
    public float x, y;
    public int G, H, pfX, pfY;
    public int F { get { return G + H; } }
}


public class PathFindManager : MonoBehaviour
{
    [SerializeField] private Status theStat;
    private Vector2 bottomLeft, topRight, startPos, targetPos;
    private List<Node> FinalNodeList;
    [SerializeField] private bool allowDiagonal, dontCrossCorner;
    [SerializeField] private LayerMask wallLayer;

    int sizeX, sizeY;
    Node[,] NodeArray;
    Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;

    private void Awake()
    {
        theStat = GetComponentInParent<Status>();
    }

    public List<Node> PathFinding(Vector3 _BL, Vector3 _TR, Vector3 _start, Vector3 _target)
    {
        bottomLeft = _BL;
        topRight = _TR;
        startPos = _start;
        targetPos = _target;

        // NodeArray의 크기 정해주고, isWall, x, y 대입
        sizeX = Mathf.RoundToInt(topRight.x - bottomLeft.x + 1);
        sizeY = Mathf.RoundToInt(topRight.y - bottomLeft.y + 1);
        NodeArray = new Node[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
                    if (col.gameObject != theStat.gameObject)
                        if ((wallLayer & (1 << col.gameObject.layer)) != 0) isWall = true;


                NodeArray[i, j] = new Node(isWall, i + bottomLeft.x, j + bottomLeft.y, i, j);
            }
        }


        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[Mathf.RoundToInt(startPos.x - bottomLeft.x), Mathf.RoundToInt(startPos.y - bottomLeft.y)];
        TargetNode = NodeArray[Mathf.RoundToInt(targetPos.x - bottomLeft.x), Mathf.RoundToInt(targetPos.y - bottomLeft.y)];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();

        while (OpenList.Count > 0)
        {
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);


            // 마지막

            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();
                return FinalNodeList;
            }

            // ↗↖↙↘
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.pfX + 1, CurNode.pfY + 1);
                OpenListAdd(CurNode.pfX - 1, CurNode.pfY + 1);
                OpenListAdd(CurNode.pfX - 1, CurNode.pfY - 1);
                OpenListAdd(CurNode.pfX + 1, CurNode.pfY - 1);
            }

            // ↑ → ↓ ←
            OpenListAdd(CurNode.pfX, CurNode.pfY + 1);
            OpenListAdd(CurNode.pfX + 1, CurNode.pfY);
            OpenListAdd(CurNode.pfX, CurNode.pfY - 1);
            OpenListAdd(CurNode.pfX - 1, CurNode.pfY);
        }
        return FinalNodeList;
    }

    void OpenListAdd(int checkX, int checkY)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= 0 && checkX < sizeX && checkY >= 0 && checkY < sizeY
        && !NodeArray[checkX, checkY].isWall
        && !ClosedList.Contains(NodeArray[checkX, checkY]))
        {
            // 대각선 허용시, 벽 사이로 통과 안됨
            if (allowDiagonal)
                if (NodeArray[CurNode.pfX, checkY].isWall
                && NodeArray[checkX, CurNode.pfY].isWall)
                    return;
            // 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
            if (dontCrossCorner)
                if (NodeArray[CurNode.pfX, checkY].isWall
                || NodeArray[checkX, CurNode.pfY].isWall)
                    return;


            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            Node NeighborNode = NodeArray[checkX, checkY];
            int MoveCost = CurNode.G + 10;


            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.pfX - TargetNode.pfX) + Mathf.Abs(NeighborNode.pfY - TargetNode.pfY)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (FinalNodeList != null)
            if (FinalNodeList.Count != 0) for (int i = 0; i < FinalNodeList.Count - 1; i++)
                    Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
    }

}


