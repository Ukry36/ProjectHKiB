using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;


public class TransferPosition : Event
{
    [SerializeField] protected Transform destination;
    [SerializeField] protected bool instant; private bool INSTBoolean => !instant;
    [ShowIf("INSTBoolean")]
    [SerializeField][Min(0)] protected float delay;
    [ShowIf("INSTBoolean")]
    [SerializeField][Min(0)] protected float innerDelay;
    [ShowIf("INSTBoolean")]
    [SerializeField] protected Color fadeColor;
    [ShowIf("INSTBoolean")]
    [SerializeField] protected Vector2 dir = Vector2.zero;
    [ShowIf("instant")]
    [SerializeField] protected bool keepX;
    [ShowIf("instant")]
    [SerializeField] protected bool keepY;

    protected override void StartEvent(Status _interactedEntity)
    {
        if (!_interactedEntity.inTransferPosition)
        {
            StartCoroutine(Cooltime());
            StartCoroutine(TransferCoroutine(_interactedEntity));
        }
    }

    protected virtual IEnumerator TransferCoroutine(Status _interactedEntity)
    {
        if (instant)
        {
            TeleportManager.instance.InstantTransferPos
            (_interactedEntity, destination.position, keepX, keepY);
        }
        else
        {
            yield return TeleportManager.instance.TransferPosCoroutine
            (_interactedEntity, dir, destination.position, delay, innerDelay, fadeColor);
        }
        EndEvent();
    }


}
