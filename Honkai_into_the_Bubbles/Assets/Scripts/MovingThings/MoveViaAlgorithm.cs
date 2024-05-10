using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class MoveViaAlgorithm : MoveSprite
{
    [SerializeField] protected bool randomMove = true; // toggle either this enemy move through specific route or just randomly
    [SerializeField] protected bool doAttack = true; // this enemy can attack
    [SerializeField] protected float NormalMoveDelay = 1; // delay before move in normal sequence
    [ShowIf("doAttack")]
    [SerializeField] protected float AggroMoveDelay = 0; //delay before move in aggro sequence
    [SerializeField] protected float turnDelay = 0; // delay before seeing player
    [ShowIf("doAttack")]
    [SerializeField] protected float attackAnimDelay = 0.5f; // delay to play attack animation fully (has to longer than attack delay)
    [ShowIf("doAttack")]
    [SerializeField] protected int followRadius = 5;  // if player is in this area, aggro sequence starts
    [ShowIf("doAttack")]
    [SerializeField] protected int endFollowRadius = 7; // if player is out of this area, Normal sequence starts
    [ShowIf("doAttack")]
    [SerializeField] protected AttackCollision attack; // yo
    [ShowIf("doAttack")]
    [SerializeField] protected GameObject beforeAttackEffectPrefab; // twinkle
    [SerializeField] protected LayerMask playerLayer; // UWU
    [SerializeField] protected LayerMask stealthPlayerLayer; // UWU
    protected Vector3 targetPos = Vector3.zero;
    [ShowIf("doAttack")]
    [SerializeField] protected Skill[] SkillArray;

    protected List<Node> ToPlayerList;
    protected Transform Player;
    protected PathFindManager pathFinder;

    [SerializeField] protected State theState;
    [ShowIf("doAttack")]
    [SerializeField] private AudioSource TinkerAudioSource;


    // instantly go in front of player before attacking
    protected void Track(Skill _skill)
    {
        for (int i = 0; i < ToPlayerList.Count - 1 && i < _skill.TrackingRadius; i++)
        {
            targetPos.x = ToPlayerList[i + 1].x;
            targetPos.y = ToPlayerList[i + 1].y;
            if (Physics2D.OverlapCircle(targetPos, .4f, wallLayer))
                break;
            movePoint.position = targetPos;
            Mover.position = movePoint.position;
        }
    }


    // tinker before attack
    protected void BeforeAttackTinker(Vector3 _offset)
    {
        Instantiate(beforeAttackEffectPrefab, this.transform.position + _offset, Quaternion.identity);
        TinkerAudioSource.Play();
    }


    // detect player if player is in specific area
    protected bool DetectPlayer(float _rad, bool _deacsth = false)
    {
        if (Physics2D.OverlapCircle(Mover.position, _rad, playerLayer))
        {
            return true;
        }
        else
        {
            if (_deacsth)
            {
                if (Physics2D.OverlapCircle(Mover.position, _rad, stealthPlayerLayer))
                    return true;
                else
                    return false;
            }
            else
            {
                if (Physics2D.OverlapCircle(Mover.position, _rad / 4f, stealthPlayerLayer))
                    return true;
                else
                    return false;
            }

        }

    }


    protected bool DetectPlayer(float _dist)
    {
        if (Physics2D.Raycast(Mover.position, applyVector, _dist, playerLayer))
        {
            return true;
        }
        return false;
    }


    // set direction to where player is 
    protected void SeePlayer(float _turnDelay = 0)
    {
        StartCoroutine(SeePlayerCoroutine(_turnDelay));
    }
    IEnumerator SeePlayerCoroutine(float _turnDelay)
    {
        yield return new WaitForSeconds(_turnDelay);
        if (Mathf.Abs(Mover.position.x - Player.position.x) >= Mathf.Abs(Mover.position.y - Player.position.y))
        {
            applyVector.y = 0f;
            if (Mover.position.x < Player.position.x)
                applyVector.x = 1f;
            else
                applyVector.x = -1f;
        }
        else
        {
            applyVector.x = 0f;
            if (Mover.position.y < Player.position.y)
                applyVector.y = 1f;
            else
                applyVector.y = -1f;
        }
        animator.SetFloat("dirX", applyVector.x);
        animator.SetFloat("dirY", applyVector.y);
    }

    protected void SeeTargetPos(Vector3 _targetPos)
    {
        if (Mathf.Abs(Mover.position.x - _targetPos.x) >= Mathf.Abs(Mover.position.y - _targetPos.y))
        {
            applyVector.y = 0f;
            if (Mover.position.x < _targetPos.x)
                applyVector.x = 1f;
            else
                applyVector.x = -1f;
        }
        else
        {
            applyVector.x = 0f;
            if (Mover.position.y < _targetPos.y)
                applyVector.y = 1f;
            else
                applyVector.y = -1f;
        }
        animator.SetFloat("dirX", applyVector.x);
        animator.SetFloat("dirY", applyVector.y);
    }


    // set the path to player
    // if player is not in the area, don't
    protected int SetPath()
    {
        if (Player.position.x <= Mover.position.x + endFollowRadius
        && Player.position.x >= Mover.position.x - endFollowRadius
        && Player.position.y <= Mover.position.y + endFollowRadius
        && Player.position.y >= Mover.position.y - endFollowRadius)
        {
            ToPlayerList = pathFinder.PathFinding(Mover.position - new Vector3(endFollowRadius, endFollowRadius, 0),
                                                Mover.position + new Vector3(endFollowRadius, endFollowRadius, 0),
                                                Mover.position, Player.position);

            return ToPlayerList.Count;
        }
        else
        {
            return 0;
        }
    }


    // set direction and movePoint randomly
    protected void RandomDirection()
    {
        applyVector.Set(0, 0);
        animator.SetFloat("dirY", 0);
        animator.SetFloat("dirX", 0);
        switch (Random.Range(0, 4))
        {
            case 0:
                applyVector.y = 1f;
                animator.SetFloat("dirY", 1f);
                break;
            case 1:
                applyVector.x = -1f;
                animator.SetFloat("dirX", -1f);
                break;
            case 2:
                applyVector.x = 1f;
                animator.SetFloat("dirX", 1f);
                break;
            case 3:
                applyVector.y = -1f;
                animator.SetFloat("dirY", -1f);
                break;
        }
    }
}
