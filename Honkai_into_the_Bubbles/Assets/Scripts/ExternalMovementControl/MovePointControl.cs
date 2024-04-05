using UnityEngine;

public class MovePointControl : MonoBehaviour
{
    [SerializeField] private Vector3 FromDto;
    [SerializeField] private Vector3 FromRto;
    [SerializeField] private Vector3 FromUto;
    [SerializeField] private Vector3 FromLto;
    private float off = 0.1f;
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.TryGetComponent(out MovePoint component))
        {
            if(this.transform.position.y - off > component.Mover.position.y)
            {
                other.transform.position += FromDto;
            }
            if(this.transform.position.x + off < component.Mover.position.x)
            {
                other.transform.position += FromRto;
            }
            if(this.transform.position.y + off < component.Mover.position.y)
            {
                other.transform.position += FromUto;
            }
            if(this.transform.position.x - off > component.Mover.position.x)
            {
                other.transform.position += FromLto;
            }
        }
    }
}
