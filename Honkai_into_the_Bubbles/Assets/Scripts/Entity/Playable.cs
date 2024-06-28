using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.U2D.Animation;

public class Playable : Entity
{
    [HideInInspector] public int spriteOverrideID;

    [HideInInspector] public bool canAttackAnim = true; // animation controls this
    [HideInInspector] public bool recieveAttackInput = true; // animation controls this
    [HideInInspector] public bool recieveDodgeInput = true; // exsists for cooltime
    [HideInInspector] public bool recieveGraffitiInput = true; // exsists for cooltime


    [BoxGroup("Attack")]
    public bool cannotAttackEffect = false; private bool CAEBoolean() { return !cannotAttackEffect; }
    [BoxGroup("Attack")]
    [ShowIf("CAEBoolean")]
    public bool isAttackEffect = false;
    [BoxGroup("Attack")]
    [ShowIf(EConditionOperator.And, "isAttackEffect", "CAEBoolean")]
    [SerializeField] protected Attack[] AttackArray;
    [BoxGroup("Attack")]
    [ShowIf(EConditionOperator.And, "isAttackEffect", "CAEBoolean")]
    [SerializeField] protected AttackCollision attackCheck;
    protected int combo;


    [BoxGroup("Dodge")]
    [SerializeField] protected bool canDodgeEffect = false;
    [BoxGroup("Dodge")]
    [ShowIf("canDodgeEffect")]
    [SerializeField] protected Animator dodgeAnim; // UWU
    [BoxGroup("Dodge")]
    [ShowIf("canDodgeEffect")]
    [SerializeField] protected SpriteRenderer dodgeSprt; // to apply theme color
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



    [BoxGroup("Graffiti")]
    [SerializeField] protected bool canGraffitiEffect = false;
    [BoxGroup("Graffiti")]
    [ShowIf("canGraffitiEffect")]
    [SerializeField] protected GraffitiSystem GS;
    [BoxGroup("Graffiti")]
    [ShowIf("canGraffitiEffect")]
    [SerializeField] protected float graffitiMaxtime = 1.5f;
    [BoxGroup("Graffiti")]
    [ShowIf("canGraffitiEffect")]
    [SerializeField] protected float graffitiCooltime = 0.3f;
    [BoxGroup("Graffiti")]
    [ShowIf("canGraffitiEffect")]
    [SerializeField] protected bool endAtGraffitiStartPoint = true;
    [BoxGroup("Graffiti")]
    [ShowIf("canGraffitiEffect")]
    [SerializeField] protected Transform GraffitiStartPoint;



    public void ChangeSpriteLibraryAsset()
    {
        SpriteLibrary.spriteLibraryAsset = Resources.Load("sdLibrary/" + ID.ToString() + "-" + spriteOverrideID.ToString(), typeof(SpriteLibraryAsset)) as SpriteLibraryAsset;
    }
}
