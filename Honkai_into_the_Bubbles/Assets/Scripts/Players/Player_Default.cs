using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Player_Default : MoveViaInput
{
    public void Start()
    {
        movePoint.parent = null;
        spriteLibrary = GetComponent<SpriteLibrary>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.SetFloat("dirX", 0);
        animator.SetFloat("dirY", -1);

        spriteOverrideID = ID;
    }

    private void Update()
    {
        // read vector2 from input sys
        moveInput = InputManager.instance.MoveInput;


        // if no WalkCoroutine is running and input exists, start walking
        if (!walking && moveInput != Vector2.zero && !freeze && !grrogying)
        {
            StartCoroutine(WalkCoroutine());
        }

        // recieve dodge input if its right timing
        if (canDodgeEffect)
        {
            dodgeInput = false;
            if (recieveDodgeInput)
                dodgeInput = InputManager.instance.DodgeInput;
            if (!dodging && dodgeInput && !freeze && !grrogying && !graffiting)
            {
                StopWalk();
                StartCoroutine(DodgeCoroutine());
            }
        }

        // recieve graffiti input if its right timing
        if (canGraffitiEffect)
        {
            startGraffitiInput = false;
            if (recieveGraffitiInput)
                startGraffitiInput = InputManager.instance.GraffitiStartInput;
            if (!graffiting && startGraffitiInput && !freeze && !grrogying && !dodging && theStat.currentGP > 0)
            {
                StopWalk();
                StartCoroutine(GraffitiCoroutine());
            }
        }

    }


    protected override void StartSkill(int _skillNum)
    {
        Debug.Log(_skillNum);
    }
}
