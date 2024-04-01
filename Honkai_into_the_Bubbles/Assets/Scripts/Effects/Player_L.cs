using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Player_L : MoveViaInput
{
    private void Awake()
    {
        movePoint.parent = null;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.SetFloat("dirX", 0);
        animator.SetFloat("dirY", -1);


        AttackArray = new Attack[] {
            new Attack(75, 0, 10, 1, false),
            new Attack(50, 1, 10, 1, false),
            new Attack(25, 0, 10, 1, false),
            new Attack(150, 1, 10, 1, true)
        };


        spriteOverrideID = ID;
    }


    private void Update()
    {
        // read vector2 from input sys
        moveInput = InputManager.instance.MoveInput;

        // if no WalkCoroutine is running and input exists, start walking
        if(!walking && moveInput != Vector2.zero && !freeze && !attacking && !dodging && !grrogying && !graffiting) 
        {
            StartCoroutine(WalkCoroutine());
        }
    

        // recieve attack input if its right timing
        attackInput = false;
        if (recieveAttackInput) 
            attackInput = InputManager.instance.AttackInput;

        // if no AttackCoroutine is running and input exists, start attacking
        if(!attacking && attackInput && !freeze && !dodging && !grrogying && !graffiting)
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
        if(!dodging && dodgeInput && !freeze && !grrogying && !graffiting)
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
        if(!graffiting && startGraffitiInput && !freeze && !grrogying && !dodging && theState.currentGP > 0)
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
        Debug.Log(_skillNum);
    }
}