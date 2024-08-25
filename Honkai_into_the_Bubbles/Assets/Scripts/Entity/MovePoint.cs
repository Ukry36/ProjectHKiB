using UnityEngine;

public class MovePoint : MonoBehaviour
{
    public Vector3 prevPos;
    public Transform mover;
    public LayerMask wallLayer;
    [SerializeField] private Transform parent = null;
    public BoxCollider2D boxCollider { get; private set; }

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
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