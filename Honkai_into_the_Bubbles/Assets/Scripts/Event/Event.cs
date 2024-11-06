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

    [SerializeField] protected Trigger triggerToExpire;

    [HideInInspector] public bool expiredLocal;

    [SerializeField] protected string initialSFX;

    public void StartEventBase(Status _interactedEntity)
    {
        if (!isCooltime)
        {
            expiredLocal = !reusable;
            AudioManager.instance.PlaySound(initialSFX, this.transform);

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
            triggerToExpire.gameObject.SetActive(false);
        }
    }

    protected virtual IEnumerator Cooltime()
    {
        if (triggerToExpire.needConfirmInput)
            triggerToExpire.triggerCollider.enabled = false;
        isCooltime = true;
        yield return new WaitForSeconds(cooltime);
        isCooltime = false;
        if (triggerToExpire.needConfirmInput)
            triggerToExpire.triggerCollider.enabled = true;
    }
}