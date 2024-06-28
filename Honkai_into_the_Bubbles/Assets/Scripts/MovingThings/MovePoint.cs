using UnityEngine;

public class MovePoint : MonoBehaviour
{
    public Vector3 prevPos;

    private void Awake()
    {
        this.transform.parent = null;
    }
}