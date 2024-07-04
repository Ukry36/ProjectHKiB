using UnityEngine;

[SerializeField]
public class StrictMoveNode
{
    public Transform node;
    public float waitTime;
    public Vector2 gazeDir;
    public int movementMultiplyer;

    public StrictMoveNode(Transform _node, float _waitTime, Vector2 _gazeDir, int _multiplyer = 1)
    {
        node = _node;
        waitTime = _waitTime;
        gazeDir = _gazeDir;
        movementMultiplyer = _multiplyer;
    }
}