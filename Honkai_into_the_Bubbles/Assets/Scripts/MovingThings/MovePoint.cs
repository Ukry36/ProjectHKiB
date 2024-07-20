using UnityEngine;

public class MovePoint : MonoBehaviour
{
    public Vector3 prevPos;
    [SerializeField] private Transform parent = null;

    private void Start()
    {
        this.transform.parent = parent;
    }
}