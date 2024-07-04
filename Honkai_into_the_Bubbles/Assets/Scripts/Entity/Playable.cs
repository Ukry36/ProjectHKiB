using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.U2D.Animation;
using System.Collections;
using Unity.VisualScripting;

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
    [ShowIf("canDodgeEffect")]
    [SerializeField] protected SpriteRenderer dodgeSprite; // to apply theme color
    [BoxGroup("Dodge")]
    [ShowIf("canDodgeEffect")]
    public int dodgeLength = 1; // max length player can dodge
    [BoxGroup("Dodge")]
    [ShowIf("canDodgeEffect")]
    [SerializeField] protected int continuousDodgeLimit = 2; // max count player can continue dodging
    [BoxGroup("Dodge")]
    [ShowIf("canDodgeEffect")]
    [SerializeField] protected float dodgeCooltime = 1f; // dodge cooltime
    [BoxGroup("Dodge")]
    [ShowIf("canDodgeEffect")]
    public bool keepDodge = false;
    [BoxGroup("Dodge")]
    [ShowIf(EConditionOperator.And, "canDodgeEffect", "keepDodge")]
    public int keepDodgeLimit = 5;
    [BoxGroup("Dodge")]
    [ShowIf(EConditionOperator.And, "canDodgeEffect", "keepDodge")]
    public float keepDodgeTimeLimit = 3f;
    [BoxGroup("Dodge")]
    [ShowIf(EConditionOperator.And, "canDodgeEffect", "keepDodge")]
    public float keepDodgeSpeed = 8f;
    protected int continuousDodgeCount = 0; // count countinuous dodge
    protected int totalDodgeCount = 0; // how many time did you dodge
    protected bool isDodgeCooltime;



    [BoxGroup("Graffiti")]
    [SerializeField] protected bool canGraffitiEffect = false;
    [BoxGroup("Graffiti")]
    [ShowIf("canGraffitiEffect")]
    public GraffitiSystem GS;
    [BoxGroup("Graffiti")]
    [ShowIf("canGraffitiEffect")]
    public float graffitiMaxtime = 1;
    [BoxGroup("Graffiti")]
    [ShowIf("canGraffitiEffect")]
    [SerializeField] protected float graffitiCooltime = 0.3f;
    [BoxGroup("Graffiti")]
    [ShowIf("canGraffitiEffect")]
    [SerializeField] protected bool endAtGraffitiStartPoint = true;
    protected bool isGraffitiCooltime;

    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public Vector3 savedInput;

    protected override void Awake()
    {
        base.Awake();

    }

    public void ChangeSpriteLibraryAsset()
    {
        SpriteLibrary.spriteLibraryAsset = Resources.Load("sdLibrary/" + ID.ToString() + "-" + spriteOverrideID.ToString(), typeof(SpriteLibraryAsset)) as SpriteLibraryAsset;
    }

    // check wall and adjust position of movepoint
    public bool MovepointAdjustCheck()
    {
        Vector3 InputX = new(savedInput.x, 0, 0);
        Vector3 InputY = new(0, savedInput.y, 0);

        if (Physics2D.OverlapCircle(MovePoint.transform.position + savedInput, .4f, 1 << LayerMask.NameToLayer("MovepointAdjust")))
        {
            if (savedInput.x == 0 || savedInput.y == 0)
            {
                return false;
            }
            else
            {
                if (Physics2D.OverlapCircle(MovePoint.transform.position + InputX, .4f, wallLayer))
                    savedInput.x = 0;

                if (Physics2D.OverlapCircle(MovePoint.transform.position + InputY, .4f, wallLayer))
                    savedInput.y = 0;

                if (Physics2D.OverlapCircle(MovePoint.transform.position + savedInput, .4f, 1 << LayerMask.NameToLayer("MovepointAdjust")))
                    return false;
            }
        }

        if (savedInput.x == 0 || savedInput.y == 0)
        {
            if (Physics2D.OverlapCircle(MovePoint.transform.position + savedInput, .4f, wallLayer))
                return true;
        }
        else // moveInput.x != 0 && moveInput.y != 0
        {


            if (Physics2D.OverlapCircle(MovePoint.transform.position + InputX, .4f, wallLayer))
                savedInput.x = 0;

            if (Physics2D.OverlapCircle(MovePoint.transform.position + InputY, .4f, wallLayer))
                savedInput.y = 0;

            if (savedInput == Vector3.zero)
                return true;

            if (savedInput.x != 0 && savedInput.y != 0)
                if (Physics2D.OverlapCircle(MovePoint.transform.position + savedInput, .4f, wallLayer))
                    MovePoint.transform.position -= InputY;
        }
        return false;
    }

    public void SetAnimDir(Vector2 _dir)
    {
        if (_dir.x != 0)
        {
            Animator.SetFloat("dirX", _dir.x);
            Animator.SetFloat("dirY", 0);
        }
        else
        {
            Animator.SetFloat("dirX", 0);
            Animator.SetFloat("dirY", _dir.y);
        }
    }

    public void DodgeCooltimeManage()
    {
        continuousDodgeCount++;
        if (continuousDodgeCount >= continuousDodgeLimit)
            StartCoroutine(DodgeCooltime());
        else
            StartCoroutine(DodgeCooltimeReset());
    }

    protected IEnumerator DodgeCooltime()
    {
        continuousDodgeCount = 0;
        isDodgeCooltime = true;
        yield return new WaitForSeconds(dodgeCooltime);
        isDodgeCooltime = false;
    }

    protected IEnumerator DodgeCooltimeReset()
    {
        int savedDodgeCount = continuousDodgeCount;
        yield return new WaitForSeconds(dodgeCooltime);
        if (continuousDodgeCount == savedDodgeCount)
            continuousDodgeCount = 0;
    }

    public IEnumerator GraffitiCooltime()
    {
        isGraffitiCooltime = true;
        yield return new WaitForSeconds(graffitiCooltime);
        isGraffitiCooltime = false;
    }

    public void SkillManage(int _skillNum)
    {
        Debug.Log(_skillNum);
    }
}
