using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Player_L : MoveViaInput
{
    private bool skill01ing = false;
    private bool skill02ing = false;
    private float skill01Maxtime = 2;
    private float skill02Maxtime = 0.5f;

    private void Awake()
    {
        movePoint.parent = null;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.SetFloat("dirX", 0);
        animator.SetFloat("dirY", -1);


        /*AttackArray = new Attack[] {
            new Attack(75, 0, 10, false),
            new Attack(50, 1, 10, false),
            new Attack(25, 0, 10, false),
            new Attack(150, 1, 10, true)
        };*/
        skill01Maxtime = GS.skillList[0].Delay;
        skill02Maxtime = GS.skillList[1].Delay;


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

        // if no AttackCoroutine is running and input exists, start attacking
        if (!dodging && dodgeInput && !freeze && !grrogying && !graffiting)
        {
            StopWalk();
            StopAttack();
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


    protected override void StartSkill(int _skillNum)
    {
        if (_skillNum == 0)
        {
            skill01ing = true;
            StartCoroutine(Skill01Coroutine(GS.skillList[0]));
        }
        else if (_skillNum == 1)
        {
            if (skill01ing || skill02ing)
            {
                skill02ing = true;
                skill01ing = false;
                StartCoroutine(Skill02Coroutine(GS.skillList[1]));
            }
        }
    }

    private IEnumerator Skill01Coroutine(Skill _skill)
    {
        attack.SetAttackInfo(_skill.DamageCoefficient, _skill.CriticalRate, _skill.Strong);
        float MT = skill01Maxtime;
        defaultSpeed /= 2;
        animator.SetBool("skill01", true);
        boxCollider.enabled = false;
        while (!attackInput && MT > 0)
        {
            yield return null;
            MT -= Time.deltaTime;
        }
        StopWalk();
        attacking = true; Debug.Log("attacking"); Debug.Log(attacking);
        animator.SetBool("skill01", false);
        boxCollider.enabled = true;
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f);
        attacking = false;
        defaultSpeed *= 2;
    }

    private IEnumerator Skill02Coroutine(Skill _skill)
    {
        attack.SetAttackInfo(_skill.DamageCoefficient, _skill.CriticalRate, _skill.Strong);
        animator.SetBool("skill02", true);
        boxCollider.enabled = false;
        attacking = true;
        float MT = skill02Maxtime;
        while (MT > 0)
        {
            yield return null;
            MT -= Time.deltaTime;
            if (parryCheck.IsTouchingLayers(LayerMask.NameToLayer("EnemyAttack")))
            {
                animator.SetTrigger("fire");
                break;
            }
        }
        boxCollider.enabled = true;
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f);
        animator.SetBool("skill02", false);
        attacking = false;

    }


}