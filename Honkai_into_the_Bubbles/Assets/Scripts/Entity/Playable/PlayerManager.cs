using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using System.Collections;
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
    public float exKeepDodgeLength = 0;
    public float exGraffitimaxtime = 0;
    public float dodgeCooltimeCoeff = 1;

    public int exHPFromEq = 0;

    public int exColdTickResistance = 0;
    public int exResistance = 0;

    public bool handLightOn = false;
    public bool dotLightOn = false;
    public bool drone = false;

    private void Start()
    {
        ActivateEquippedEffect(new int[] { 0 });
    }

    public IEnumerator DieSequence()
    {
        if (SceneManager.GetActiveScene().name != "Hyperion")
        {
            yield return TeleportManager.instance.LoadSceneCoroutine
            ("Hyperion", theStat, Vector2.down, new Vector3(13, -10, 0), 0.5f, 0.1f, Color.black);
        }
        else
        {
            yield return TeleportManager.instance.TransferPosCoroutine
            (theStat, Vector2.down, new Vector3(13, -10, 0), 0.5f, 0.1f, Color.black);
        }
        theStat.HPControl(1000);
        theStat.GPControl(1000);
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
            if (EquippedEffects[i].ID != 0) exHPFromEq += 25;
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

        theStat.HPCapControl();

        if (drone)
        {
            if (currentDrone == null || !currentDrone.activeSelf)
            {
                currentDrone = PoolManager.instance.ReuseGameObject(Drone, this.transform.position, quaternion.identity);
            }
            else
            {
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
        exKeepDodgeLength = 0;
        exGraffitimaxtime = 0;
        dodgeCooltimeCoeff = 1;
        exColdTickResistance = 0;
        handLightOn = false;
        dotLightOn = false;
        drone = false;
        exHPFromEq = 0;

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

    public void FriendlyInstantTransfer(Vector3 _way)
    {
        if (drone)
            currentDrone.transform.position += _way;
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
            exColdTickResistance = 100;
        }
        else
        {
            exColdTickResistance += 50;
        }
    }

    private void Active10018Blanket(bool _isPrime)
    {
        if (_isPrime)
        {
            exColdTickResistance = 100;
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
            exKeepDodgeSpeedCoeff *= 2f;
            exKeepDodgeLength += 2;
            dodgeCooltimeCoeff *= 1.2f;
        }
        forcedKeepDodge = true;
        forcedCanDodge = true;
    }

    private void Activate10024Zero(bool _isPrime)
    {
        if (_isPrime)
        {
            dodgeCooltimeCoeff *= 0.8f;
        }
        exDodgeLength += 1;
        forcedCanDodge = true;
    }

}
