using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickDamage : MonoBehaviour
{
    public Color hitColor;
    public string[] SFX;
    public LayerMask damageLayer;
    public int Damage;
    public bool Strong;
    public float Cooltime;
    public float Coeff;
    public float Delay;
    public TickDamageType Type;
    public enum TickDamageType
    {
        Cold, Poison, Flame
    }
    [HideInInspector] public bool isCooltime;

    protected void OnTriggerStay2D(Collider2D other)
    {
        if ((damageLayer & (1 << other.gameObject.layer)) != 0)
        {
            if (other.gameObject.TryGetComponent(out Status component))
            {
                StartCoroutine(component.TickDamageCoroutine(this));
            }
        }
    }

}