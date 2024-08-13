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

    public void Hit(Attack _attackInfo)
    {
        Vector3 attackOrigin = _attackInfo.theStat.transform.position;

        int trueDMG = _attackInfo.theStat.ATK * _attackInfo.DamageCoefficient / 100;

        bool critical = Random.Range(1, 101) < _attackInfo.BaseCriticalRate + _attackInfo.theStat.CritRate;

        trueDMG += critical ? trueDMG * _attackInfo.theStat.CritDMG / 100 : 0;

        trueDMG = trueDMG > DEF ? trueDMG - DEF : 0;

        entity.Hit(attackOrigin);

        if (!invincible)
        {
            StartCoroutine(entity.HitLightShine(_attackInfo.hitColor));
            StartCoroutine(entity.HitInvincible());
            HPControl(-trueDMG);
            Debug.Log(trueDMG);
        }

        int Coeff = _attackInfo.Strong - Mass;

        if (!superArmor && Coeff > 0)
        {
            entity.Knockback(attackOrigin, Coeff);
        }
    }

    public IEnumerator TickDamageCoroutine(TickDamage _damageInfo)
    {
        for (int i = 0; i < _damageInfo.Coeff; i++)
        {
            int trueDMG = _damageInfo.Damage;
            if (isPlayer)
            {
                if (_damageInfo.Type == TickDamage.TickDamageType.Cold)
                {
                    if (PlayerManager.instance.halfImuneToColdTick)
                    {
                        trueDMG /= 2;
                    }
                    else if (PlayerManager.instance.imuneToColdTick)
                    {
                        yield break;
                    }
                }
                else
                {
                    // onother damagetype imune calc
                }
            }

            if (!invincible)
            {
                StartCoroutine(entity.HitLightShine(_damageInfo.hitColor));
                HPControl(-trueDMG, _damageInfo.SFX);
            }

            if (!superArmor && _damageInfo.Strong)
            {
                entity.Knockback(this.transform.position, 1);
                StartCoroutine(entity.HitInvincible());
            }
            yield return new WaitForSeconds(_damageInfo.Delay);
        }
    }

    public void SetHitAnimObject() => entity = GetComponentsInChildren<Entity>(false)[0];

    public void HPControl(int _o, string[] _customSFX = null, bool _silence = false)
    {
        currentHP += _o;
        if (currentHP > maxHP) currentHP = maxHP;

        if (currentHP <= 0)
        {
            currentHP = 0;
            entity.Die();
        }

        if (!_silence)
        {
            if (_customSFX == null)
            {
                if (_o < 0)
                {
                    AudioManager.instance.PlaySound(entity.hitSFX, this.transform);
                }
                else if (_o > 0)
                {
                    AudioManager.instance.PlaySound(AudioManager.instance.hpSFX, this.transform);
                }
            }
            else
            {
                AudioManager.instance.PlaySound(_customSFX, this.transform);
            }
        }
    }

    public void GPControl(int _o, string[] _customSFX = null, bool _silence = false)
    {
        currentGP += _o;
        if (currentGP > maxGP) currentGP = maxGP;

        if (currentGP < 0) currentGP = 0;

        if (!_silence)
        {
            if (_customSFX == null)
            {
                if (_o < 0)
                {
                    AudioManager.instance.PlaySound(AudioManager.instance.gpDrainSFX, this.transform);
                }
                else if (_o > 0)
                {
                    AudioManager.instance.PlaySound(AudioManager.instance.gpSFX, this.transform);
                }
            }
            else
            {
                AudioManager.instance.PlaySound(_customSFX, this.transform);
            }
        }
    }
}
