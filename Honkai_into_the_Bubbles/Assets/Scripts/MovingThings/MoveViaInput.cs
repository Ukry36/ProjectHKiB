using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public abstract class MoveViaInput : MoveSprite
{
    public bool isAttackEffect = false;
    public bool cannotAttackEffect = false;
    [HideInInspector] public int spriteOverrideID;
    protected List<Color> dodgeColors = new();
    [SerializeField] protected State theState;


    protected Vector2 moveInput; // vector2 from new input sys
    protected bool attackInput;
    protected bool dodgeInput;
    protected bool startGraffitiInput;


    protected bool attacking = false; // if true, AttackCoroutine is running
    protected bool dodging = false; // if true, DodgeCoroutine is running
    protected bool graffiting = false; // if true, GraffitiCoroutine is running


    [HideInInspector] public bool canAttackAnim = true; // animation controls this
    [HideInInspector] public bool recieveAttackInput = true; // animation controls this
    [HideInInspector] public bool recieveDodgeInput = true; // exsists for cooltime
    [HideInInspector] public bool recieveGraffitiInput = true; // exsists for cooltime
    

    [SerializeField] protected Attack[] AttackArray;
    [SerializeField] protected AttackCollision attack;
    protected int combo;


    [SerializeField] protected Animator dodgeAnim; // UWU
    [SerializeField] protected SpriteRenderer dodgeSprt; // to apply theme color
    [SerializeField] protected int dodgeLength = 1; // max length player can dodge
    [SerializeField] protected int continuousDodgeLimit = 2; // max count player can continue dodging
    [SerializeField] protected float dodgeCooltime = 1f; // dodge cooltime
    [SerializeField] protected bool keepDodge = false;
    [SerializeField] protected int keepDodgeLimit = 5;
    [SerializeField] protected float keepDodgeTimeLimit = 3f;
    protected int continuousDodgeCount = 0; // count countinuous dodge
    protected int totalDodgeCount = 0; // how many time did you dodge


    [SerializeField] protected GraffitiSystem GS;
    [SerializeField] protected float graffitiMaxtime = 1.5f;
    [SerializeField] protected float graffitiCooltime = 0.3f;
    [SerializeField] protected bool endAtGraffitiStartPoint = true;
    [SerializeField] protected Transform GraffitiStartPoint;
    
    

    public virtual void ChangeThemeColor(Color _c1, Color _c2)
    {
        dodgeColors.Clear();
        dodgeColors.Add(_c1);
        dodgeColors.Add(_c2);
    }

    public void ChangeSpriteLibraryAsset()
    {
        spriteLibrary.spriteLibraryAsset = Resources.Load("sdLibrary/" + ID.ToString() + "-" + spriteOverrideID.ToString(), typeof(SpriteLibraryAsset)) as SpriteLibraryAsset;
    }
    
    
    protected void RawVectorSetDirection()
    {
        GetRawVector();
        SetDir();
    }

    protected void GetRawVector()
    {
        // turn vector(applyVector) from normalized to raw
        if(applyVector.x * applyVector.y != 0) 
            applyVector = new Vector2(applyVector.x / Mathf.Abs(applyVector.x), applyVector.y / Mathf.Abs(applyVector.y));
    }

    protected void SetDir(bool _byRawInput = false)
    {
        // determine direction of player in animtion
        if(applyVector.x != 0)
        {
            if (_byRawInput)
                animator.SetFloat("dirX", moveInput.x);
            else
                animator.SetFloat("dirX", applyVector.x);
            animator.SetFloat("dirY", 0);
        } 
        else
        {
            animator.SetFloat("dirX", 0);
            if (_byRawInput)
                animator.SetFloat("dirY", moveInput.y);
            else
                animator.SetFloat("dirY", applyVector.y);
        }
    }


// manage wall detecting including diagonal move
    protected bool DetectWall()
    {
        if (applyVector.x == 0 || applyVector.y == 0)
        {
            if (Physics2D.OverlapCircle(movePoint.position, .4f, wallLayer))
                return true;
        }
        else
        {
            if (Physics2D.OverlapCircle(movePoint.position - new Vector3(0, applyVector.y, 0), .4f, wallLayer))
            {
                movePoint.position -= new Vector3(applyVector.x, 0, 0);
                applyVector.x = 0;

            }
                
            if (Physics2D.OverlapCircle(movePoint.position - new Vector3(applyVector.x, 0, 0), .4f, wallLayer))
            {
                movePoint.position -= new Vector3(0, applyVector.y, 0);
                applyVector.y = 0;
            }
                
            if (applyVector.x == 0 && applyVector.y == 0)
                return true;
            
            if (applyVector.x != 0 && applyVector.y != 0)
                if (Physics2D.OverlapCircle(movePoint.position, .4f, wallLayer))
                    movePoint.position -= new Vector3(0, applyVector.y, 0);
        }
        return false;
    }


// walks until there is no input
    protected IEnumerator WalkCoroutine()
    {
        walking = true;

        while(moveInput != Vector2.zero && !freeze && stopWalkboolean)
        {
            // set movespeed default
            // while pressing sprint, movespeed doubles
            if (InputManager.instance.SprintInput)
                moveSpeed = defaultSpeed * 2;
            else
                moveSpeed = defaultSpeed;


            // save moveInput in applyVector
            applyVector = moveInput; 
            GetRawVector();


            // move destination forward
            movePoint.position += (Vector3)applyVector; 
            yield return null;
            if (DetectWall())
            {
                movePoint.position -= (Vector3)applyVector; 
                SetDir(true); // 수정 요망!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                break;
            }
            SetDir();
            

            // move toward destination
            // if stopWalkboolean is false, skip sequencial movement
            animator.SetBool("walk", true);
            while(Vector3.Distance(Mover.position, movePoint.position) >= .05f)
            {
                yield return null;
                if (!stopWalkboolean)
                    break;
                Mover.position = Vector3.MoveTowards(Mover.position, movePoint.position, moveSpeed * Time.deltaTime); 
            }
            Mover.position = movePoint.position;
        }
        animator.SetBool("walk", false);
        walking = false;
        stopWalkboolean = true;
    }


    // essential function when escaping attackCoroutine
    protected virtual void StopAttacking()
    {
        attacking = false;
        stopAttackboolean = true;
        recieveAttackInput = false;
        canAttackAnim = false;
        combo = 0;
        animator.SetInteger("attack", combo);
    }

// attack (ends attack)
    protected IEnumerator AttackCoroutine(Attack _attack)
    {  
        attacking = true;
        attack.SetAttackInfo(_attack.DamageCoefficient, _attack.CriticalRate, _attack.Strong, _attack.GraffitiPoint);


        if(moveInput != Vector2.zero)
            applyVector = moveInput; 
        GetRawVector();


        // move until moveLimit or wall
        // in combo3 command, don't move
        if(moveInput != Vector2.zero) 
        {
            for (int i = 0; i < _attack.TrackingRadius; i++)
            {
                movePoint.position += (Vector3)applyVector; 
                yield return null;
                if (DetectWall())
                {
                    movePoint.position -= (Vector3)applyVector; 
                    break;
                }
                
                Mover.position = movePoint.position;
            }
        }


        SetDir();
        // start attack sequence of desired combo
        animator.SetInteger("attack", combo);

    
        // if there is input at proper timing, get into next attack
        // == yield return new WaitUntil(() => recieveAttackInput && !canAttackAnim);
        while (!recieveAttackInput || canAttackAnim)
        {
            yield return null;
            
            if (!stopAttackboolean) // if dodge is reserved, end attacking
            {
                StopAttacking();
                yield break;
            }
        }
        

        bool flag = false;
        while(recieveAttackInput) // from recieveAtkInput point to end of animation
        {
            yield return null;
            if (attackInput) // if there is input, next attack is reserved
                flag = true;
            
            if (!stopAttackboolean) // if dodge is reserved, end attacking
            {
                StopAttacking();
                yield break;
            }

            if (flag && canAttackAnim) // if main attack motion is over and already have input, stop wating
                break;
        }


        if (flag) // if attack is reserved up there, start next attack
        {
            if (combo == AttackArray.Length)
                combo = 0;
            combo += 1;
            StartCoroutine(AttackCoroutine(AttackArray[combo - 1]));
        }
        else // else, quit attacking
        {
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f);
            StopAttacking();
        }
    }


    // dodge (ends attck or walk)
    protected IEnumerator DodgeCoroutine()
    {
        dodgeAnim.SetBool("keepDodge", keepDodge);
        dodging = true;
        boxCollider.enabled = false;


        // alter color
        dodgeSprt.color = dodgeColors[totalDodgeCount++ % 2];


        // activate start dodge animation and wait until it ends
        // also detect combo3 command
        dodgeAnim.SetTrigger("startDodge");
        yield return null;
        while (dodgeAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99f)
        {
            yield return null;
        }


        // move
        boxCollider.enabled = true;
        if (!keepDodge)
        {
            if(moveInput != Vector2.zero)
                applyVector = moveInput; 
            RawVectorSetDirection();
            

            // move to where dodgeLength reaches further (if no moveInput, dodge backward) 
            Vector2 apv = applyVector * dodgeLength;
            if(moveInput != Vector2.zero)
            {
                for (int i = 0; i < dodgeLength; i++)
                {
                    if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(apv.x - i, apv.y - i, 0f), .4f, wallLayer)) 
                    {
                        movePoint.position += new Vector3(apv.x - i, apv.y - i, 0f);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < dodgeLength; i++)
                {
                    if(!Physics2D.OverlapCircle(movePoint.position - new Vector3(apv.x - i, apv.y - i, 0f), .4f, wallLayer)) 
                    {
                        movePoint.position -= new Vector3(apv.x - i, apv.y - i, 0f);
                        break;
                    }
                }
            }
            Mover.position = movePoint.position; //new Vector3((int)movePoint.position.x, (int)movePoint.position.y);
        }
        else
        {
            for (int i = 0; i < keepDodgeLimit; i++)
            {
                yield return null;
                if (!InputManager.instance.DodgeProgressInput)
                    break;

                float limitTime = 0;
                while (moveInput == Vector2.zero)
                {
                    yield return null;
                    limitTime += Time.deltaTime;
                    if (limitTime > keepDodgeTimeLimit)
                        break;
                }
                if (limitTime > keepDodgeTimeLimit)
                    break;

                if(moveInput != Vector2.zero)
                    applyVector = moveInput;
                RawVectorSetDirection();
                if (Physics2D.OverlapCircle(movePoint.position + (Vector3)applyVector, .4f, wallLayer)) 
                {
                    i--;
                    continue;
                }
                movePoint.position += (Vector3)applyVector; 
                while(Vector3.Distance(Mover.position, movePoint.position) >= .05f)
                {
                    Mover.position = Vector3.MoveTowards(Mover.position, movePoint.position, moveSpeed * 2 * Time.deltaTime); 
                    yield return null;
                }
            }
        }


        // activate end dodge animation and wait until it ends
        // also detect combo3 command
        dodgeAnim.SetTrigger("endDodge");
        for (int i = 0; i < 12; i++)
        {
            yield return null;
        }


        // end dodge and manage cooltime
        continuousDodgeCount++;
        if (continuousDodgeCount >= continuousDodgeLimit)
            StartCoroutine(DodgeCooltimeCoroutine());
        else
            StartCoroutine(DodgeCooltimeResetCoroutine());
        dodging = false;
        canAttackAnim = true;
    }

// dodge cooltime
    protected IEnumerator DodgeCooltimeCoroutine()
    {
        continuousDodgeCount = 0;
        recieveDodgeInput = false;
        yield return new WaitForSeconds(dodgeCooltime);
        recieveDodgeInput = true;
    }

// reset dodge cooltime if player hasn't dodged for a while from first dodge 
    protected IEnumerator DodgeCooltimeResetCoroutine()
    {
        yield return new WaitForSeconds(dodgeCooltime);
        if (continuousDodgeCount == 1 && !dodging)
            continuousDodgeCount = 0;
    }


    protected IEnumerator GraffitiCoroutine()
    {
        if (Physics2D.OverlapCircle(movePoint.position, .4f, wallLayer + GS.WallForGraffitiLayer)) 
            yield break;
        graffiting = true;
        grrogying = true;
        GS.StartGraffiti();
        dodgeAnim.SetBool("keepDodge", true);
        dodgeSprt.color = dodgeColors[totalDodgeCount++ % 2];
        dodgeAnim.SetTrigger("startDodge");
        boxCollider.enabled = false;
        yield return null;
        if (dodgeAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f)
            boxCollider.enabled = true;

        while(true)
        {
            yield return null;
            if (InputManager.instance.GraffitiEndInput)
                break;
            
            if (theState.currentGP > 0)
            {
                if (moveInput == Vector2.zero)
                applyVector = moveInput;

                if (moveInput != Vector2.zero && applyVector == Vector2.zero)
                    applyVector = moveInput;
                else
                    continue;

                if (applyVector.x != 0)
                    applyVector.y = 0;

                if (Physics2D.OverlapCircle(movePoint.position + (Vector3)applyVector, .4f, wallLayer + GS.WallForGraffitiLayer)) 
                    continue;
            
                movePoint.position += (Vector3)applyVector;
                Mover.position = movePoint.position;
            }
            
        }
        dodgeAnim.SetTrigger("endDodge");
        dodgeAnim.SetBool("keepDodge", false);
        if (endAtGraffitiStartPoint)
        {
            Mover.position = GraffitiStartPoint.position;
            movePoint.position = GraffitiStartPoint.position;
        }
        StartSkill(GS.EndGraffiti());
        StartCoroutine(GraffitiCooltimeCoroutine());
        yield return new WaitUntil(() => dodgeAnim.GetCurrentAnimatorStateInfo(0).IsName("sd_0010_dodge_default_empty"));
        graffiting = false;
        grrogying = false;
    }


// graffiti cooltime
    protected IEnumerator GraffitiCooltimeCoroutine()
    {
        recieveGraffitiInput = false;
        yield return new WaitForSeconds(graffitiCooltime);
        recieveGraffitiInput = true;
    }

    protected abstract void StartSkill(int _skillNum);
}
