using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    public LayerMask damageLayer;
    [HideInInspector] public int DMG;
    [HideInInspector] public int GP;
    [HideInInspector] public bool Strong;
    [HideInInspector] public bool Crit;
    private Vector3 GrrogyDir;
    [HideInInspector] public bool Override;
    [HideInInspector] public int dmgCoeffOverride = 0;
    [HideInInspector] public int critRateOverride = 0;
    [HideInInspector] public int GPOverride = 0;
    [HideInInspector] public bool strongOverride = false;
    public State theState;

    protected BoxCollider2D boxCollider2D;
    protected Vector3 prevPos;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        prevPos = this.transform.position;
    }

    public void SetAttackInfo(int _DmgCoeff, int _CritRate, bool _Strong, int _GP = 0)
    {
        DMG = theState.ATK * _DmgCoeff / 100;
        Crit = false;
        if (Random.Range(1, 101) < _CritRate + theState.CritRate)
        {
            DMG += DMG * theState.CritDMG / 100;
            Crit = true;
        }
        GP = _GP;
        Strong = _Strong;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        float x, y;
        if ((damageLayer & (1 << other.gameObject.layer)) != 0)
        {
            if (Strong)
            {
                GrrogyDir = other.transform.position - prevPos;

                if (GrrogyDir.x != 0)
                    x = GrrogyDir.x / Mathf.Abs(GrrogyDir.x);
                else
                    x = 0;

                if (GrrogyDir.y != 0)
                    y = GrrogyDir.y / Mathf.Abs(GrrogyDir.y);
                else
                    y = 0;

                if (x * y != 0)
                {
                    if (Mathf.Abs(x) >= Mathf.Abs(y) * 2)
                        y = 0;

                    if (Mathf.Abs(y) >= Mathf.Abs(x) * 2)
                        x = 0;
                }

                if (x == 0 && y == 0)
                {
                    if (Random.value >= 0.5f)
                        x = 1f;
                    else
                        x = -1f;
                    if (Random.value >= 0.5f)
                        y = 1f;
                    else
                        y = -1f;
                }

                GrrogyDir = new Vector3(x, y, 0);
            }
            else
            {
                GrrogyDir = Vector3.zero;
            }

            if (Override)
            {
                SetAttackInfo(dmgCoeffOverride, critRateOverride, strongOverride, GPOverride);
                Override = false;
            }
            theState.GPControl(GP);
            if (other.gameObject.TryGetComponent(out State component))
            {
                component.Hit(DMG, Crit, Strong, GrrogyDir); Debug.Log(DMG);
            }
        }
    }
}
