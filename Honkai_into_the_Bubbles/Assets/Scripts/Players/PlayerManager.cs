using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            EveryEffects = GetComponentsInChildren<Playable>(true);
            theStat = GetComponent<Status>();
            prevEffect = Array.Find(EveryEffects, de => de.ID == 0);
            theStat.SetHitAnimObject();
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    private int[] equippedEffectIDs = { 0 };
    private Playable[] EveryEffects;
    private List<Playable> EquippedEffects = new();
    private Playable prevEffect;

    private int attackActivateState;  // -1: not attack effect, 0~3: EquippedEffects[0~3] is attack effect

    [HideInInspector] public List<Color> ThemeColors { get; private set; } = new();

    private Status theStat;


    public bool isStealth = false;
    public int invincibleTimeCoeff = 1;
    public float exSpeedCoeff = 1;
    public bool forcedCanDodge = false;
    public bool forcedKeepDodge = false;
    public int exDodgeLength = 0;
    public int exContinuousDodgeLimit = 0;
    public float exKeepDodgeSpeedCoeff = 1;
    public float exGraffitimaxtime = 0;


    private void Start()
    {
        ActivateEquippedEffect(new int[] { 0 });
    }

    public void ActivateEquippedEffect(int[] _effectIDs)
    {
        // distinct realEpdEffIDs from EquipmentManager
        equippedEffectIDs = _effectIDs.Distinct().ToArray();


        // deactivate previous passives
        ActivatePassives(false);


        // equippedEffectIDs -> EquippedEffects
        EquippedEffects.Clear();
        for (int j = 0; j < equippedEffectIDs.Length; j++)
            for (int i = 0; i < EveryEffects.Length; i++)
                if (EveryEffects[i].ID == equippedEffectIDs[j])
                    EquippedEffects.Add(EveryEffects[i]);


        // if there is attack effect in EquippedEffects, 
        //first attack effect will be activated
        attackActivateState = -1;
        for (int i = 0; i < EquippedEffects.Count; i++)
        {
            if (EquippedEffects[i].isAttackEffect)
            {
                attackActivateState = i;
                break;
            }
        }


        // if prime effect cannot attack, cancel activating attack effect
        if (EquippedEffects[0].cannotAttackEffect)
            attackActivateState = -1;


        // if activate attack effect is not in prime slot, change its sprite with prime slot effect's one
        if (attackActivateState > 0)
            EquippedEffects[attackActivateState].spriteOverrideID = EquippedEffects[0].ID;
        else
            EquippedEffects[0].spriteOverrideID = EquippedEffects[0].ID;


        ThemeColors = EquippedEffects[0].ThemeColors;


        // thus, if cannotAttackEffect is in prime slot or there is no attack effect, activate effect in prime slot
        // else, activate attack effect in proper slot
        // while activating effects, sprite will change according to its spriteOverrideID
        if (attackActivateState >= 0)
        {
            EquippedEffects[attackActivateState].ChangeSpriteLibraryAsset();
            if (EquippedEffects[attackActivateState].ID != prevEffect.ID)
            {
                EquippedEffects[attackActivateState].gameObject.SetActive(true);
                prevEffect.gameObject.SetActive(false);
                prevEffect = EquippedEffects[attackActivateState];
            }
        }
        else
        {
            EquippedEffects[0].ChangeSpriteLibraryAsset();
            if (EquippedEffects[0].ID != prevEffect.ID)
            {
                EquippedEffects[0].gameObject.SetActive(true);
                prevEffect.gameObject.SetActive(false);
                prevEffect = EquippedEffects[0];
            }
        }

        theStat.SetHitAnimObject();

        // activate passives
        ActivatePassives(true);
    }


    private void ActivatePassives(bool _activate)
    {
        bool primeC = true;
        for (int i = 0; i < EquippedEffects.Count; i++)
        {
            if (i == 1)
                primeC = false;
            switch (EquippedEffects[i].ID)
            {
                case 10000:
                    Activate10000Potion(_activate, primeC);
                    break;
                case 10007:
                    Activate10007Cat(_activate, primeC);
                    break;
                default:
                    break;
            }
        }
    }


    private void Activate10000Potion(bool _activate, bool _isPrime)
    {
        if (_activate)
        {
            if (_isPrime)
            {
                Debug.Log("canGrafitti");
            }
            InputManager.instance.canSprint = true;
        }
        else
        {
            InputManager.instance.canSprint = false;
        }
    }

    private void Activate10007Cat(bool _activate, bool _isPrime)
    {
        if (_activate)
        {
            if (_isPrime)
            {
                isStealth = true;
            }
            else
            {
                Debug.Log("somethin");
            }
        }
        else
        {
            isStealth = false;
        }
    }

}
