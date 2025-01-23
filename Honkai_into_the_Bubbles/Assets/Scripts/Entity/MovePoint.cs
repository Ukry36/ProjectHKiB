using UnityEngine;

public class MovePoint : MonoBehaviour
{
    public Vector3 prevPos;
    public Transform mover;
    public LayerMask wallLayer;
    [SerializeField] private Transform parent = null;
    public BoxCollider2D boxCollider { get; private set; }

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        mover = this.transform.parent;
    }
    private void OnEnable()
    {
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