using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Color hitColor;
    public string hitSFX;
    public string initialSFX;
    public Status theStat;
    public LayerMask damageLayer;
    public int DamageCoefficient; // Coefficient of atk
    public int BaseCriticalRate; // possibility of critical
    public int GraffitiPoint; // amount of GraffitiPoint to obtain
    public int Strong; // cause hit motion
    public int TrackingRadius; // teleport in front of enemy on tracking area

    protected virtual void OnEnable()
    {
        AudioManager.instance.PlaySound(initialSFX, this.transform);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if ((damageLayer & (1 << other.gameObject.layer)) != 0)
        {
            theStat.GPControl(GraffitiPoint);
            if (other.gameObject.TryGetComponent(out Status component))
            {
                AudioManager.instance.PlaySound(hitSFX, this.transform);
                component.Hit(this);
            }
        }
    }
}
