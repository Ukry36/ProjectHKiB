using UnityEngine;

public class MovePointControl : MonoBehaviour
{
    [SerializeField] private Vector3 FromDto;
    [SerializeField] private Vector3 FromRto;
    [SerializeField] private Vector3 FromUto;
    [SerializeField] private Vector3 FromLto;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out MovePoint component))
        {
            if (other.transform.position.y > component.prevPos.y && FromDto != Vector3.zero)
            {
                other.transform.position += FromDto;
                component.prevPos = other.transform.position;
            }

            if (other.transform.position.x < component.prevPos.x && FromRto != Vector3.zero)
            {
                other.transform.position += FromRto;
                component.prevPos = other.transform.position;
            }

            if (other.transform.position.y < component.prevPos.y && FromUto != Vector3.zero)
            {
                other.transform.position += FromUto;
                component.prevPos = other.transform.position;
            }

            if (other.transform.position.x > component.prevPos.x && FromLto != Vector3.zero)
            {
                other.transform.position += FromLto;
                component.prevPos = other.transform.position;
            }
        }
    }
}
