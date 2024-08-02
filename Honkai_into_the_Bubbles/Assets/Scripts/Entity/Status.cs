using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public bool isPlayer = false;
    public bool superArmor = false; // no knockback
    public bool invincible = false; // no damage
    public int maxHP = 100;
    public int currentHP;
    public int maxGP = 20;
    public int currentGP;

    public int ATK = 100;
    public int DEF = 0;
    public int CritRate = 0;
    public int CritDMG = 10;
    public int Mass = 1;
    [HideInInspector] public Entity entity;

    private void Awake()
    {
        entity = GetComponentInChildren<Entity>();
    }

    public void Hit(int _dmg, bool _crit, int _strong, Vector3 _attackOrigin)
    {
        entity.Hit(_attackOrigin);
        int trueDmg = _dmg > DEF ? _dmg - DEF : 0;

        if (!invincible)
        {
            StartCoroutine(entity.HitCoroutine());
            HPControl(-trueDmg);
        }

        int Coeff = _strong - Mass;

        if (!superArmor && Coeff > 0)
        {
            entity.Knockback(_attackOrigin, Coeff);
        }
    }

    public void SetHitAnimObject()
    {
        entity = GetComponentsInChildren<Entity>(false)[0];
    }

    public void HPControl(int _o)
    {
        currentHP += _o;
        if (currentHP > maxHP)
            currentHP = maxHP;
        if (currentHP <= 0)
        {
            currentHP = 0;
            entity.Die();
        }
    }

    public void GPControl(int _o)
    {
        currentGP += _o;
        if (currentGP > maxGP)
            currentGP = maxGP;
        if (currentGP < 0)
            currentGP = 0;
    }
}
