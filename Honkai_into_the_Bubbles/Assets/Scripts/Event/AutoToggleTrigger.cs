using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoToggleTrigger : Trigger
{
    [SerializeField] protected string triggerOffSFX;
    void Update()
    {
        if (activated) targetEvent.StartEventBase(null);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!targetEvent.expiredLocal && !needConfirmInput && (interactLayer & (1 << other.gameObject.layer)) != 0)
        {
            activated = !activated;
            PlaySFX();
        }
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
        if (!targetEvent.expiredLocal && needConfirmInput && (interactLayer & (1 << other.gameObject.layer)) != 0)
        {
            if (InputManager.instance.ConfirmInput && other.TryGetComponent(out Status stat))
            {
                activated = !activated;
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

}
