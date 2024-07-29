using UnityEngine;

public class MovePointControlFast : MonoBehaviour
{
    [SerializeField] private Vector3 FromDto;
    [SerializeField] private Vector3 FromRto;
    [SerializeField] private Vector3 FromUto;
    [SerializeField] private Vector3 FromLto;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out MovePoint component))
        {
            if (other.transform.position.y > component.mover.position.y && FromDto != Vector3.zero)
                other.transform.position += FromDto;

            if (other.transform.position.x < component.mover.position.x && FromRto != Vector3.zero)
                other.transform.position += FromRto;

            if (other.transform.position.y < component.mover.position.y && FromUto != Vector3.zero)
                other.transform.position += FromUto;

            if (other.transform.position.x > component.mover.position.x && FromLto != Vector3.zero)
                other.transform.position += FromLto;
        }
    }
}
