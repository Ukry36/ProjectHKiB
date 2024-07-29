using UnityEngine;

public class MovePoint : MonoBehaviour
{
    public Vector3 prevPos;
    public Transform mover;
    public LayerMask wallLayer;
    [SerializeField] private Transform parent = null;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        mover = this.transform.parent;
        this.transform.parent = parent;
    }
    /*
        private void Update()
        {
            if (boxCollider.IsTouchingLayers(wallLayer))
            {
                this.transform.position = prevPos;
            }
        }*/

}