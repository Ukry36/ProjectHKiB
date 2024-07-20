using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] protected LayerMask interactLayer;
    [SerializeField] protected Event targetEvent;

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (!targetEvent.expiredLocal && (interactLayer & (1 << other.gameObject.layer)) != 0)
        {
            targetEvent.StartEvent();
        }
    }
}
