using UnityEngine;
using NaughtyAttributes;
using UnityEngine.U2D.Animation;
using System.Collections;

public class Playable : Entity
{
    [HideInInspector] public int spriteOverrideID;

    [BoxGroup("Attack")]
    public bool cannotAttackEffect = false; private bool CAEBoolean => !cannotAttackEffect;
    [BoxGroup("Attack")]
    [ShowIf("CAEBoolean")]
    public bool isAttackEffect = false;
    [BoxGroup("Attack")]
    [ShowIf(EConditionOperator.And, "isAttackEffect", "CAEBoolean")]
    public Attack[] AttackArray; // insert primer attacks only


    [BoxGroup("Dodge")]
    [SerializeField] protected bool canDodgeEffect = false;
    [BoxGroup("Dodge")]
    [SerializeField] protected SpriteRenderer dodgeSprite; // to apply theme color
    [BoxGroup("Dodge")]
    public int dodgeLength = 1; // max length player can dodge
    [BoxGroup("Dodge")]
    [SerializeField] protected int continuousDodgeLimit = 2; // max count player can continue dodging
    [BoxGroup("Dodge")]
    [SerializeField] protected float dodgeCooltime = 1.5f; // dodge cooltime
    [BoxGroup("Dodge")]
    public bool keepDodge = false;
    [BoxGroup("Dodge")]
    public int keepDodgeLimit = 6;
    [BoxGroup("Dodge")]
    public float keepDodgeTimeLimit = 3f;
    [BoxGroup("Dodge")]
    public float keepDodgeSpeed = 12f;
    protected int continuousDodgeCount = 0; // count countinuous dodge
    protected int totalDodgeCount = 0; // how many time did you dodge
    protected bool isDodgeCooltime;



    [BoxGroup("Graffiti")]
    [SerializeField] protected bool canGraffitiEffect = false;
    [HideInInspector] public GraffitiSystem GS;
    [BoxGroup("Graffiti")]
    public float graffitiMaxtime = 0.15f;
    [BoxGroup("Graffiti")]
    [SerializeField] protected float graffitiCooltime = 0.3f;
    [BoxGroup("Graffiti")]
    [SerializeField] protected bool endAtGraffitiStartPoint = true;
    protected bool isGraffitiCooltime;

    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public Vector3 savedInput;

    [HideInInspector] public bool cannotDodge;
    [HideInInspector] public bool cannotGraffiti;

    public Playable_StateMachine stateMachine;

    protected override void Awake()
    {
        base.Awake();
        GS = GetComponent<GraffitiSystem>();
        stateMachine = new Playable_StateMachine();
    }

    public virtual void ChangeSpriteLibraryAsset()
    {
        SpriteLibrary.spriteLibraryAsset = Resources.Load("sdLibrary/" + ID.ToString() + "-" + spriteOverrideID.ToString(), typeof(SpriteLibraryAsset)) as SpriteLibraryAsset;
    }

    // check wall and adjust position of movepoint
    public bool MovepointAdjustCheck()
    {
        Vector3 InputX = new(savedInput.x, 0, 0);
        Vector3 InputY = new(0, savedInput.y, 0);
        if (savedInput.x == 0 || savedInput.y == 0) // non diagonal
        {
            return PointWallCheck(MovePoint.transform.position + savedInput);
        }
        else // moveInput.x != 0 && moveInput.y != 0    (diagonal)
        {
            if (PointWallCheck(MovePoint.transform.position + InputX))
                savedInput.x = 0;

            if (PointWallCheck(MovePoint.transform.position + InputY))
                savedInput.y = 0;

            if (savedInput == Vector3.zero)
                return true;

            if (savedInput.x != 0 && savedInput.y != 0)
                if (PointWallCheck(MovePoint.transform.position + savedInput))
                    MovePoint.transform.position -= InputY;
        }
        return false;
    }


    public void DodgeCooltimeManage()
    {
        continuousDodgeCount++;
        if (continuousDodgeCount >= continuousDodgeLimit + PlayerManager.instance.exContinuousDodgeLimit)
            StartCoroutine(DodgeCooltime());
        else
            StartCoroutine(DodgeCooltimeReset());
    }

    protected IEnumerator DodgeCooltime()
    {
        continuousDodgeCount = 0;
        isDodgeCooltime = true;
        yield return new WaitForSeconds(dodgeCooltime * PlayerManager.instance.dodgeCooltimeCoeff);
        isDodgeCooltime = false;
    }

    protected IEnumerator DodgeCooltimeReset()
    {
        int savedDodgeCount = continuousDodgeCount;
        yield return new WaitForSeconds(dodgeCooltime * PlayerManager.instance.dodgeCooltimeCoeff);
        if (continuousDodgeCount == savedDodgeCount)
            continuousDodgeCount = 0;
    }

    public IEnumerator GraffitiCooltime()
    {
        isGraffitiCooltime = true;
        yield return new WaitForSeconds(graffitiCooltime);
        isGraffitiCooltime = false;
    }

    public virtual void SkillManage(int[] _result)
    {
        Debug.Log(_result[0]);
    }

    public virtual void GraffitiFailManage(int _usedGP)
    {
        theStat.HPControl(_usedGP * 10);
    }

    public override void StationalActivateManage(bool _Enter)
    {
        if (_Enter)
        {
            if (PlayerManager.instance.dotLightOn)
                PlayerManager.instance.DotLight.SetActive(true);
            if (PlayerManager.instance.handLightOn)
                PlayerManager.instance.HandLight.SetActive(true);
        }
        else
        {
            if (PlayerManager.instance.dotLightOn)
                PlayerManager.instance.DotLight.SetActive(false);
            if (PlayerManager.instance.handLightOn)
                PlayerManager.instance.HandLight.SetActive(false);
        }
    }
}
