using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    [SerializeField] protected bool reusable;
    [SerializeField] protected bool saveable;

    [SerializeField] protected GameObject triggerToExpire;

    [HideInInspector] public bool expiredLocal;

    [SerializeField] protected string initialSFX;

    public virtual void StartEvent(Status _interactedEntity)
    {
        expiredLocal = !reusable;
        AudioManager.instance.PlaySound(initialSFX, this.transform);
    }

    public virtual void EndEvent()
    {
        if (saveable)
        {
            // save this objects info in database
        }
        if (expiredLocal)
        {
            Destroy(triggerToExpire);
            Destroy(this);
        }
    }
}