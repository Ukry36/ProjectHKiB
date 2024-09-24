using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;

public class Status : MonoBehaviour
{
    public bool isPlayer = false;
    public bool superArmor = false; // no knockback
    public bool invincible = false; // no damage
    [SerializeField] private int maxHP = 100;
    [ReadOnly] public int currentMaxHP;
    public int CurrentHP { get; private set; }
    [SerializeField] private int maxGP = 20;
    [ReadOnly] public int currentMaxGP;
    public int CurrentGP { get; private set; }

    public int playHitSFXDamage = 50;

    public int ATK = 100;
    public int DEF = 0;
    public int CritRate = 0;
    public int CritDMG = 10;
    public int Mass = 1;
    public int Size = 1;
    [HideInInspector] public Entity entity;
    [HideInInspector] public bool inTransferPosition;

    [SerializeField][MinValue(0)][MaxValue(100)] private int Resistance = 0;
    [HideInInspector] public int ResistanceExternal = 0;

    [SerializeField][MinValue(0)][MaxValue(100)] private int ColdTickResistance = 0;
    [SerializeField][MinValue(0)][MaxValue(100)] private int FlameTickResistance = 0;

    [HideInInspector] public int ColdTickResistanceExternal = 0;
    [HideInInspector] public int FlameTickResistanceExternal = 0;

    private void Awake()
    {
        entity = GetComponentInChildren<Entity>();
        currentMaxHP = maxHP;
        CurrentHP = maxHP;
        currentMaxGP = maxGP;
        CurrentGP = maxGP;
    }

    public void TransferPositionInvincible(float _time)
    {
        StopCoroutine(nameof(TransferPositionInvincibleCoroutine));
        StartCoroutine(TransferPositionInvincibleCoroutine(_time));
    }

    private IEnumerator TransferPositionInvincibleCoroutine(float _time)
    {
        inTransferPosition = true;
        invincible = true;
        superArmor = true;
        yield return new WaitForSeconds(_time);
        inTransferPosition = false;
        invincible = false;
        superArmor = false;
    }

    public void Hit(Attack _attackInfo)
    {
        Vector3 attackOrigin = _attackInfo.theStat.transform.position;

        int trueDMG = _attackInfo.theStat.ATK * _attackInfo.DamageCoefficient / 100;

        bool critical = UnityEngine.Random.Range(1, 101) < _attackInfo.BaseCriticalRate + _attackInfo.theStat.CritRate;
        int resistance = Resistance + ResistanceExternal;

        trueDMG += critical ? trueDMG * _attackInfo.theStat.CritDMG / 100 : 0;
        trueDMG -= DEF;
        trueDMG = trueDMG * (100 - resistance) / 100;
        trueDMG = trueDMG < 0 ? 0 : trueDMG;

        entity.Hit(attackOrigin);

        if (!invincible && this.gameObject.activeSelf)
        {
            //StartCoroutine(entity.HitLightShine(_attackInfo.hitColor));
            ParticleSystem.MainModule pm = PoolManager.instance.ReuseGameObject(PoolManager.instance.hitParticle, this.transform.position + Vector3.up, quaternion.identity).GetComponent<ParticleSystem>().main;
            pm.startColor = _attackInfo.hitColor;
            StartCoroutine(entity.HitInvincible());
            HPControl(-trueDMG);
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
            int resistance = 0;
            switch (_damageInfo.Type)
            {
                case TickDamage.TickDamageType.Cold:
                    if (isPlayer) resistance += PlayerManager.instance.exColdTickResistance;
                    resistance += ColdTickResistance + ColdTickResistanceExternal;
                    break;
                case TickDamage.TickDamageType.Flame:
                    resistance += FlameTickResistance + FlameTickResistanceExternal;
                    break;
                default:
                    break;
            }

            int trueDMG = _damageInfo.Damage * (100 - resistance) / 100;
            trueDMG = trueDMG * (100 - Resistance) / 100;
            trueDMG = trueDMG < 0 ? 0 : trueDMG;

            if (!invincible && trueDMG > 0)
            {
                ParticleSystem.MainModule pm = PoolManager.instance.ReuseGameObject(PoolManager.instance.hitParticle, this.transform.position + Vector3.up, quaternion.identity).GetComponent<ParticleSystem>().main;
                pm.startColor = _damageInfo.hitColor;
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

    public void HPCapControl()
    {
        int newMHP = maxHP + PlayerManager.instance.exHPFromEq;
        float ratio = (float)CurrentHP / currentMaxHP;
        currentMaxHP = newMHP;
        CurrentHP = (int)(currentMaxHP * ratio);
    }

    public void HPControl(int _o, string[] _customSFX = null, bool _silence = false)
    {
        CurrentHP += _o;
        if (CurrentHP > currentMaxHP) CurrentHP = currentMaxHP;

        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            entity.Die();
        }

        if (!_silence)
        {
            if (_customSFX == null)
            {
                if (_o <= -playHitSFXDamage)
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
        CurrentGP += _o;
        if (CurrentGP > currentMaxGP) CurrentGP = currentMaxGP;

        if (CurrentGP < 0) CurrentGP = 0;

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
