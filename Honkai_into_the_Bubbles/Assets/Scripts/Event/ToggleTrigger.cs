using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTrigger : Trigger
{
    [SerializeField] private Event OffEvent;
    [SerializeField] protected string triggerOffSFX;
    private bool activated;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!targetEvent.expiredLocal && !needConfirmInput && (interactLayer & (1 << other.gameObject.layer)) != 0)
        {
            if (other.TryGetComponent(out Status stat))
            {
                activated = true;
                StartEvent(stat);
                PlaySFX();
            }

        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (!targetEvent.expiredLocal && !needConfirmInput && (interactLayer & (1 << other.gameObject.layer)) != 0)
        {
            if (other.TryGetComponent(out Status stat))
            {
                activated = false;
                StartEvent(stat);
                PlaySFX();
            }

        }
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
        if (!targetEvent.expiredLocal && needConfirmInput && (interactLayer & (1 << other.gameObject.layer)) != 0)
        {
            if (InputManager.instance.ConfirmInput && other.TryGetComponent(out Status stat))
            {
                activated = !activated;
                StartEvent(stat);
                PlaySFX();
            }
        }
    }

    protected override void PlaySFX()
    {
        if (activated)
            AudioManager.instance.PlaySound(triggerSFX, this.gameObject.transform);
        else
            AudioManager.instance.PlaySound(triggerOffSFX, this.gameObject.transform);
    }

    private void StartEvent(Status _stat)
    {
        if (activated)
        {
            if (OffEvent != null) targetEvent.EndEvent();
            if (targetEvent != null) targetEvent.StartEventBase(_stat);
        }
        else
        {
            if (targetEvent != null) targetEvent.EndEvent();
            if (OffEvent != null) OffEvent.StartEventBase(_stat);
        }

    }

}
