using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Unity.Mathematics;

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

    public Status theStat;

    [SerializeField] private GameObject Drone;
    private GameObject currentDrone;
    public GameObject HandLight;
    public GameObject DotLight;

    public bool canSprint = false;
    public bool isStealth = false;
    public int invincibleTimeCoeff = 1;
    public float exSpeedCoeff = 1;
    public bool forcedCanDodge = false;
    public bool forcedKeepDodge = false;
    public int exDodgeLength = 0;
    public int exContinuousDodgeLimit = 0;
    public float exKeepDodgeSpeedCoeff = 1;
    public float exGraffitimaxtime = 0;
    public float dodgeCooltimeCoeff = 1;

    public bool imuneToColdTick = false;
    public bool halfImuneToColdTick = false;

    public bool handLightOn = false;
    public bool dotLightOn = false;
    public bool drone = false;


    private void Start()
    {
        ActivateEquippedEffect(new int[] { 0 });
    }

    public void ActivateEquippedEffect(int[] _effectIDs)
    {
        // distinct realEpdEffIDs from EquipmentManager
        equippedEffectIDs = _effectIDs.Distinct().ToArray();

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

                EquippedEffects[attackActivateState].SetAnimDir(prevEffect.moveDir);

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

                EquippedEffects[0].SetAnimDir(prevEffect.moveDir);

                prevEffect = EquippedEffects[0];
            }
        }

        theStat.SetHitAnimObject();

        // activate passives
        ActivatePassives();
    }


    private void ActivatePassives()
    {
        DeactivatePassives();
        bool primeC = true;
        for (int i = 0; i < EquippedEffects.Count; i++)
        {
            if (i == 1)
                primeC = false;
            switch (EquippedEffects[i].ID)
            {
                case 10000:
                    Activate10000Potion(primeC);
                    break;
                case 10005:
                    Activate10005Mechanic01(primeC);
                    break;
                case 10007:
                    Activate10007Cat(primeC);
                    break;
                case 10016:
                    Active10016LongPadding(primeC);
                    break;
                case 10018:
                    Active10018Blanket(primeC);
                    break;
                case 10019:
                    Active10019HandLight(primeC);
                    break;
                case 10023:
                    Activate10023Infinity(primeC);
                    break;
                case 10024:
                    Activate10024Zero(primeC);
                    break;
                default:
                    break;
            }
        }

        if (drone)
        {
            if (currentDrone == null || !currentDrone.activeSelf)
            {
                currentDrone = PoolManager.instance.ReuseGameObject(Drone, this.transform.position + new Vector3(20, 20), quaternion.identity);
            }
            else
            {
                Debug.Log(currentDrone);
                currentDrone.GetComponentInChildren<Drone>().CancelDisable();
            }

        }
        else
        {
            if (currentDrone != null && currentDrone.activeSelf)
            {
                currentDrone.GetComponentInChildren<Drone>().DisableSequence();
            }
        }
    }

    private void DeactivatePassives()
    {
        canSprint = false;
        isStealth = false;
        invincibleTimeCoeff = 1;
        exSpeedCoeff = 1;
        forcedCanDodge = false;
        forcedKeepDodge = false;
        exDodgeLength = 0;
        exContinuousDodgeLimit = 0;
        exKeepDodgeSpeedCoeff = 1;
        exGraffitimaxtime = 0;
        dodgeCooltimeCoeff = 1;
        imuneToColdTick = false;
        halfImuneToColdTick = false;
        handLightOn = false;
        dotLightOn = false;
        drone = false;

        if (DotLight.activeSelf) DotLight.SetActive(false);
        if (HandLight.activeSelf) HandLight.SetActive(false);
    }


    public void FriendlyResetWhenTransferposition()
    {
        if (drone)
        {
            currentDrone.SetActive(false);
            currentDrone = PoolManager.instance.ReuseGameObject(Drone, this.transform.position, quaternion.identity);
        }
    }


    private void Activate10000Potion(bool _isPrime)
    {
        if (_isPrime)
        {
            Debug.Log("canGrafitti");
        }
        canSprint = true;
    }

    private void Activate10005Mechanic01(bool _isPrime)
    {
        if (_isPrime)
        {
            Debug.Log("canTransform");
        }

        drone = true;
    }

    private void Activate10007Cat(bool _isPrime)
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

    private void Active10016LongPadding(bool _isPrime)
    {
        if (_isPrime)
        {
            imuneToColdTick = true;
        }
        else
        {
            halfImuneToColdTick = true;
        }
    }

    private void Active10018Blanket(bool _isPrime)
    {
        if (_isPrime)
        {
            imuneToColdTick = true;
        }
    }

    private void Active10019HandLight(bool _isPrime)
    {
        if (_isPrime)
        {
            handLightOn = true;
        }
        dotLightOn = true;
    }

    private void Activate10023Infinity(bool _isPrime)
    {
        if (_isPrime)
        {
            forcedKeepDodge = true;
        }
        else
        {
            exContinuousDodgeLimit += 1;
            Debug.Log(exContinuousDodgeLimit);
        }
        forcedCanDodge = true;
    }

    private void Activate10024Zero(bool _isPrime)
    {
        if (_isPrime)
        {
            exDodgeLength += 1;
        }
        else
        {
            dodgeCooltimeCoeff /= 2;
        }
        forcedCanDodge = true;
    }

}
