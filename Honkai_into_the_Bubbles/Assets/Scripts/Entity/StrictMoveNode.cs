using UnityEngine;

[SerializeField]
public class StrictMoveNode
{
    public Transform node; // destination (only used with PathfindMove)
    public float waitTime;
    public Vector2 gazeDir; // next animation dir and move dir in DirMove, animation dir in PathfindMove
    public int movementMultiplyer; // (only used with DirMove)

    public StrictMoveNode(Transform _node, float _waitTime, Vector2 _gazeDir, int _multiplyer = 1)
    {
        node = _node;
        waitTime = _waitTime;
        gazeDir = _gazeDir;
        movementMultiplyer = _multiplyer;
    }
}