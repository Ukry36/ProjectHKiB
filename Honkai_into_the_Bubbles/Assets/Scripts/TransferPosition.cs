using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TransferPosition : MonoBehaviour
{
    [SerializeField] protected Transform destination;
    [SerializeField] protected LayerMask canTransferLayer;
    [SerializeField] protected float delay;
    [SerializeField] protected float innerDelay;
    [SerializeField] protected Color fadeColor;
    [SerializeField] protected Vector2 dir = Vector2.zero;

    private bool can = true;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out Status component))
        {
            if ((canTransferLayer & (1 << component.gameObject.layer)) != 0)
            {
                if (can)
                {
                    can = false;
                    StartCoroutine(TransferCoroutine(component));
                }
            }
        }
    }

    protected virtual IEnumerator TransferCoroutine(Status component)
    {
        if (component.isPlayer)
        {
            InputManager.instance.StopUIInput(true);
            InputManager.instance.StopPlayerInput(true);
            MenuManager.instance.SetFadeColor(fadeColor);
        }

        yield return MenuManager.instance.FadeCoroutine(1, delay);

        component.transform.position = destination.position;
        component.entity.MovePoint.transform.position = destination.position;
        if (component.isPlayer)
            CameraManager.instance.StrictMovement(destination.position, this.transform.position);
        if (dir != Vector2.zero)
            component.entity.SetAnimDir(dir);

        yield return new WaitForSeconds(innerDelay);
        PlayerManager.instance.FriendlyResetWhenTransferposition();

        if (component.isPlayer)
        {
            yield return MenuManager.instance.FadeCoroutine(0, delay);
            InputManager.instance.StopPlayerInput(false);
            InputManager.instance.StopUIInput(false);
        }
        can = true;
    }


}
