using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Event : MonoBehaviour
{
    [SerializeField] protected bool reusable;
    [SerializeField] protected bool saveable;
    [SerializeField] protected float cooltime;
    [HideInInspector] public bool isCooltime;

    [SerializeField] protected GameObject triggerToExpire;

    [HideInInspector] public bool expiredLocal;

    [SerializeField] protected string initialSFX;

    public virtual void StartEventBase(Status _interactedEntity)
    {
        if (!isCooltime)
        {
            isCooltime = true;
            expiredLocal = !reusable;
            AudioManager.instance.PlaySound(initialSFX, this.transform);
            if (reusable)
            {
                StartCoroutine(Cooltime());
            }
            StartEvent(_interactedEntity);
        }
    }

    protected virtual void StartEvent(Status _interactedEntity)
    {
        Debug.LogError("ERROR: No StartEvent!");
    }

    public virtual void EndEvent()
    {
        if (saveable)
        {
            // save this objects info in database
        }
        if (expiredLocal)
        {
            triggerToExpire.SetActive(false);
        }
    }

    private IEnumerator Cooltime()
    {
        yield return new WaitForSeconds(cooltime);
        isCooltime = false;
    }
}