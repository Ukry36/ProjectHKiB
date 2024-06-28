using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Delta : MoveViaInput
{
    private bool willDodge = false; // if true, Dodge is reserved to end attack or walk
    private bool startAtCombo3 = false;


    private void Awake()
    {
        movePoint.parent = null;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.SetFloat("dirX", 0);
        animator.SetFloat("dirY", -1);


        AttackArray = new Attack[] {
            new Attack(100, 0, 10, false),
            new Attack(50, 1, 10, false),
            new Attack(100, 2, 10, false),
            new Attack(200, 2, 10, false),
            new Attack(300, 3, 10, true)
        };


        spriteOverrideID = ID;
    }


    private void Update()
    {
        // read vector2 from input sys
        moveInput = InputManager.instance.MoveInput;
        // if no WalkCoroutine is running and input exists, start walking
        if (!walking && moveInput != Vector2.zero && !freeze && !attacking && !dodging && !grrogying && !graffiting)
        {
            StartCoroutine(WalkCoroutine());
        }


        // recieve attack input if its right timing
        attackInput = false;
        if (recieveAttackInput)
            attackInput = InputManager.instance.AttackInput;
        // if no AttackCoroutine is running and input exists, start attacking
        if (!attacking && attackInput && !freeze && !dodging && !grrogying && !graffiting)
        {
            combo = 1;
            StopWalk();
            StartCoroutine(AttackCoroutine(AttackArray[combo - 1]));
        }


        // recieve dodge input if its right timing
        dodgeInput = false;
        if (recieveDodgeInput)
            dodgeInput = InputManager.instance.DodgeInput;
        if (!dodging && dodgeInput && !freeze && !grrogying && !graffiting)
        {
            StopWalk();
            StopAttack();
            willDodge = true;
            recieveAttackInput = false;
            canAttackAnim = false;
            StartCoroutine(DodgeCoroutine());
        }


        // recieve graffiti input if its right timing
        startGraffitiInput = false;
        if (recieveGraffitiInput)
            startGraffitiInput = InputManager.instance.GraffitiStartInput;
        if (!graffiting && startGraffitiInput && !freeze && !grrogying && !dodging && theStat.currentGP > 0)
        {
            StopWalk();
            StopAttack();
            recieveAttackInput = false;
            canAttackAnim = false;
            StartCoroutine(GraffitiCoroutine());
        }
    }


    // essential function when escaping attackCoroutine
    protected override void StopAttacking()
    {
        attacking = false;
        stopAttackboolean = true;
        recieveAttackInput = false;
        canAttackAnim = false;
        combo = 0;
        animator.SetInteger("attack", combo);
    }


    // attack (ends attack)
    private new IEnumerator AttackCoroutine(Attack _attack)
    {
        attacking = true;
        attack.SetAttackInfo(_attack.DamageCoefficient, _attack.CriticalRate, _attack.Strong);


        if (moveInput != Vector2.zero)
            applyVector = moveInput;
        GetRawVector();


        // move until moveLimit or wall
        // in combo3 command, don't move
        if (moveInput != Vector2.zero && !startAtCombo3)
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
        else
        {
            startAtCombo3 = false; // if startAtCombo3 == true, startAtCombo3 = false
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
                if (willDodge && combo == 3 || combo == 4)
                    startAtCombo3 = true;
                StopAttacking();
                yield break;
            }
        }


        bool flag = false;
        while (recieveAttackInput) // from recieveAtkInput point to end of animation
        {
            yield return null;
            if (attackInput) // if there is input, next attack is reserved
                flag = true;

            if (!stopAttackboolean) // if dodge is reserved, end attacking
            {
                if (willDodge && combo == 3 || combo == 4)
                    startAtCombo3 = true;
                StopAttacking();
                yield break;
            }

            if (flag && canAttackAnim) // if main attack motion is over and already have input, stop wating
                break;
        }


        if (flag) // if attack is reserved up there, start next attack
        {
            if (combo == 5)
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
    private new IEnumerator DodgeCoroutine()
    {
        dodgeAnim.SetBool("keepDodge", keepDodge);
        dodging = true;
        willDodge = false;
        boxCollider.enabled = false;


        // alter color
        dodgeSprt.color = dodgeColors[(totalDodgeCount++) % 2];


        // activate start dodge animation and wait until it ends
        // also detect combo3 command
        dodgeAnim.SetTrigger("startDodge");
        yield return null;
        bool flag = false;
        while (dodgeAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99f)
        {
            if (startAtCombo3 && InputManager.instance.AttackInput)
            {
                flag = true;
            }
            yield return null;
        }


        // move
        boxCollider.enabled = true;
        if (!keepDodge)
        {
            if (moveInput != Vector2.zero)
                applyVector = moveInput;
            RawVectorSetDirection();

            // move to where dodgeLength reaches further (if no moveInput, dodge backward) 
            Vector2 apv = applyVector * dodgeLength;
            if (moveInput != Vector2.zero)
            {
                for (int i = 0; i < dodgeLength; i++)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(apv.x - i, apv.y - i, 0f), .4f, wallLayer))
                    {
                        movePoint.position += new Vector3(apv.x - i, apv.y - i, 0f);
                        break;
                    }
                }
                Mover.position = movePoint.position;
            }
            else
            {
                for (int i = 0; i < dodgeLength; i++)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position - new Vector3(apv.x - i, apv.y - i, 0f), .4f, wallLayer))
                    {
                        movePoint.position -= new Vector3(apv.x - i, apv.y - i, 0f);
                        break;
                    }
                }
                Mover.position = movePoint.position;
            }
        }
        else
        {
            for (int i = 0; i < keepDodgeLimit; i++)
            {
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

                if (moveInput != Vector2.zero)
                    applyVector = moveInput;
                RawVectorSetDirection();
                if (Physics2D.OverlapCircle(movePoint.position + new Vector3(applyVector.x, applyVector.y, 0f), .4f, wallLayer))
                {
                    i--;
                    continue;
                }
                movePoint.position += new Vector3(applyVector.x, applyVector.y, 0f);
                while (Vector3.Distance(Mover.position, movePoint.position) >= .05f)
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
            if (startAtCombo3 && InputManager.instance.AttackInput)
            {
                flag = true;
            }
            yield return null;
        }
        if (flag)
        {
            for (int i = 0; i < 6; i++)
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


        // if combo3 command is detected, start attack(combo3)
        if (flag)
        {
            combo = 3;
            continuousDodgeCount = 0;
            StartCoroutine(AttackCoroutine(AttackArray[combo - 1]));
        }
    }


    protected override void StartSkill(int _skillNum)
    {
        Debug.Log(_skillNum);
    }
}