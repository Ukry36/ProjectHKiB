using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferPosition : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.TryGetComponent(out MovePoint component))
        {
            other.transform.position += destination.position - this.transform.position;
            component.Mover.position += destination.position - this.transform.position;
        }
    }
}
