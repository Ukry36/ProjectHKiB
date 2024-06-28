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
            InputManager.instance.StopPlayerInput(true);
            ScreenManager.instance.SetFadeColor(fadeColor);
        }

        yield return ScreenManager.instance.FadeCoroutine(1, delay);

        component.transform.position += destination.position - this.transform.position;
        component.entity.MovePoint.transform.position += destination.position - this.transform.position;
        if (component.isPlayer)
            CameraManager.instance.StrictMovement(destination.position - this.transform.position);
        if (dir != Vector2.zero)
        {
            component.entity.Animator.SetFloat("dirX", dir.x);
            component.entity.Animator.SetFloat("dirY", dir.y);
        }

        yield return new WaitForSeconds(innerDelay);

        if (component.isPlayer)
        {
            yield return ScreenManager.instance.FadeCoroutine(0, delay);
            InputManager.instance.StopPlayerInput(false);
        }
        can = true;
    }


}
