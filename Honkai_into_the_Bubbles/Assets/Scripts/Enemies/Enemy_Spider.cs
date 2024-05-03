using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spider : MoveViaAlgorithm
{
    private WaitForSeconds wait = new WaitForSeconds(0.5f);


    // Start is called before the first frame update
    private void Start()
    {
        movePoint.parent = null;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Player = FindObjectOfType<PlayerManager>().transform;
        pathFinder = GetComponent<PathFindManager>();
        moveSpeed = defaultSpeed;

        SkillArray = new Skill[] {
            new Skill(0, 1, 3, 3f, 0.3f, true)
        }; //upper skill high priority (not nessesory just for easy recognizing)


        theState.SetHitAnimObject();
        StartCoroutine(NormalBehaviourCoroutine());
    }


    IEnumerator AggroBehaviourCoroutine()
    {
        while(true)
        {
            while(grrogying)
                yield return wait;
            

            if (!DetectPlayer(endFollowRadius))
            {
                StartCoroutine(NormalBehaviourCoroutine());
                break;
            }


            if (SetPath() <= 1)
            {
                StartCoroutine(NormalBehaviourCoroutine());
                break;
            }
            else
            {
                targetPos.x = ToPlayerList[1].x;
                targetPos.y = ToPlayerList[1].y;
            }

            SeeTargetPos();

            if (SkillArray[0].CanSkill && DetectPlayer(SkillArray[0].DetectRadius, true))
            {
                StartCoroutine(Skill01Coroutine());
                break;
            }

            yield return new WaitForSeconds(AggroMoveDelay);
            walking = true;
            // if there is a wall, wait
            if(Physics2D.OverlapCircle(targetPos, .4f, wallLayer))
            {
                yield return wait;
                walking = false;
                continue;
            }
            
            // move destination forward
            movePoint.position = targetPos; 
            // move toward destination
            // if stopWalkboolean is false, skip sequencial movement
            animator.SetBool("walk", true);
            while(Vector3.Distance(Mover.position, movePoint.position) >= .05f)
            {
                if (!stopWalkboolean)
                    break;
                Mover.position = Vector3.MoveTowards(Mover.position, movePoint.position, moveSpeed * Time.deltaTime); 
                yield return null;
            }
            Mover.position = movePoint.position;
            walking = false;
            animator.SetBool("walk", false);
            stopWalkboolean = true;
        }
    }


    IEnumerator NormalBehaviourCoroutine()
    {
        while(true)
        {
            while(grrogying)
                yield return wait;
            

            if (doAttack && DetectPlayer(followRadius) && SetPath() > 1)
            {
                yield return null;
                StartCoroutine(AggroBehaviourCoroutine());
                break;
            }
            //ToPlayerList.Clear();


            RandomDirection();
            yield return new WaitForSeconds(NormalMoveDelay);


            walking = true;
            // if there is a wall, wait
            if(Physics2D.OverlapCircle(movePoint.position + new Vector3(applyVector.x, applyVector.y, 0f), .4f, wallLayer)) 
            {
                yield return wait;
                continue;
            }


            // move destination forward
            movePoint.position += (Vector3)applyVector; 
            // move toward destination
            // if stopWalkboolean is false, skip sequencial movement
            animator.SetBool("walk", true);
            while(Vector3.Distance(Mover.position, movePoint.position) >= .05f)
            {
                if (!stopWalkboolean)
                    break;
                Mover.position = Vector3.MoveTowards(Mover.position, movePoint.position, moveSpeed * Time.deltaTime); 
                yield return null;
            }
            Mover.position = movePoint.position;
            walking = false;
            animator.SetBool("walk", false);
            stopWalkboolean = true;
        }
    }


    private void StopAttacking()
    {   
        animator.SetFloat("skill", 0);
        stopAttackboolean = true;
        StartCoroutine(AggroBehaviourCoroutine());
    }


    IEnumerator Skill01Coroutine()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);


        while (grrogying)
            yield return wait;
        if (!stopAttackboolean)
        {
            StopAttacking();
            yield break;
        }


        SeePlayer();
        animator.SetFloat("skill", 1f);


        for (float time = 0; time < attackAnimDelay - SkillArray[0].Delay; time += Time.deltaTime)
        {
            if (!stopAttackboolean)
            {
                StopAttacking();
                yield break;
            }
            yield return null;
        }
        BeforeAttackTinker(Vector3.zero);
        yield return new WaitForSeconds(SkillArray[0].Delay);


        SkillArray[0].CanSkill = false;
        animator.SetTrigger("fire");
        yield return null;
        yield return null;
        for (int i = 0; i < 8; i++) //Rush until wall
        {
            if(Physics2D.OverlapCircle(Mover.position + new Vector3(applyVector.x, applyVector.y, 0), .4f, wallLayer)) 
                continue;
            movePoint.position += new Vector3(applyVector.x, applyVector.y, 0);
            Mover.position = movePoint.position;
        }
        
        Die();
    }


}
