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
        Vector3 Way = destination.position - _interactedEntity.entity.MovePoint.transform.position;

        if (instant)
        {
            Way.x = keepX ? 0 : Way.x;
            Way.y = keepY ? 0 : Way.y;
            dir = Vector2.zero;
            if (_interactedEntity.isPlayer)
            {
                PlayerManager.instance.FriendlyInstantTransfer(Way);
                _interactedEntity.TransferPositionInvincible(0.1f);
            }
            _interactedEntity.transform.position += Way;
            _interactedEntity.entity.MovePoint.transform.position += Way;
        }
        else
        {
            if (_interactedEntity.isPlayer)
            {
                _interactedEntity.TransferPositionInvincible(delay * 2 + innerDelay);
                InputManager.instance.StopUIInput(true);
                InputManager.instance.StopPlayerInput(true);
                MenuManager.instance.SetFadeColor(fadeColor);
                yield return MenuManager.instance.FadeCoroutine(1, delay);
            }
            else
            {
                _interactedEntity.TransferPositionInvincible(innerDelay);
            }
            _interactedEntity.transform.position = destination.position;
            _interactedEntity.entity.MovePoint.transform.position = destination.position;
        }



        if (_interactedEntity.isPlayer)
        {
            CameraManager.instance.StrictMovement(Way, _interactedEntity.entity.MovePoint.transform.position);
        }


        if (dir != Vector2.zero)
            _interactedEntity.entity.SetAnimDir(dir);

        yield return new WaitForSeconds(innerDelay);


        if (_interactedEntity.isPlayer && !instant)
        {
            PlayerManager.instance.FriendlyResetWhenTransferposition();
            yield return MenuManager.instance.FadeCoroutine(0, delay);
            InputManager.instance.StopPlayerInput(false);
            InputManager.instance.StopUIInput(false);
        }
        EndEvent();
    }


}
