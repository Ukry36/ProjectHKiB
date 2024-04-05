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
        if(!walking && moveInput != Vector2.zero && !freeze && !grrogying) 
        {
            StartCoroutine(WalkCoroutine());
        }
    }


    protected override void StartSkill(int _skillNum)
    {
        Debug.Log(_skillNum);
    }
}
