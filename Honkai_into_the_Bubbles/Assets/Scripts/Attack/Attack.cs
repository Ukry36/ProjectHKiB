using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Status theStat;
    public LayerMask damageLayer;
    public int DamageCoefficient; // Coefficient of atk
    public int TrackingRadius; // teleport in front of enemy on tracking area
    public int BaseCriticalRate; // possibility of critical
    public int GraffitiPoint; // amount of GraffitiPoint to obtain
    public int Strong; // cause hit motion
    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        int trueDMG = theStat.ATK * DamageCoefficient / 100;
        bool critical = Random.Range(1, 101) < BaseCriticalRate + theStat.CritRate;

        if (critical)
            trueDMG += trueDMG * theStat.CritDMG / 100;

        theStat.GPControl(GraffitiPoint);


        if ((damageLayer & (1 << other.gameObject.layer)) != 0)
        {
            if (other.gameObject.TryGetComponent(out Status component))
            {
                component.Hit(trueDMG, critical, Strong, this.transform.position);
            }
        }
    }
}
