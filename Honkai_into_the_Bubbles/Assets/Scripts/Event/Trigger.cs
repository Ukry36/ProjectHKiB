using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] protected LayerMask interactLayer;
    [SerializeField] protected Event targetEvent;
    [SerializeField] protected bool needConfirmInput;

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!targetEvent.expiredLocal && !needConfirmInput && (interactLayer & (1 << other.gameObject.layer)) != 0)
        {
            targetEvent.StartEvent(other.TryGetComponent(out Status stat) ? stat : null);
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (!targetEvent.expiredLocal && needConfirmInput && (interactLayer & (1 << other.gameObject.layer)) != 0)
        {
            if (InputManager.instance.ConfirmInput && other.TryGetComponent(out Status stat))
            {
                targetEvent.StartEvent(stat);
            }
        }
    }
}
