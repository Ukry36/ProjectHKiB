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
    protected Vector3 targetPos = Vector3.zero;
    [ShowIf("doAttack")]
    [SerializeField] protected Skill[] SkillArray;

    protected List<Node> ToPlayerList;
    protected Transform Player;
    protected State PlayerState;
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
        float rad = PlayerState.isStealth && !_deacsth ? _rad / 4 : _rad;

        if (Physics2D.OverlapCircle(Mover.position, rad, playerLayer))
        {
            return true;
        }
        return false;
    }


    protected bool DetectPlayer(float _dist, int _maxErr, bool _deacsth = false)
    {
        Vector3 v;
        if (applyVector.x == 0)
            v = new Vector3(1, 0);
        else
            v = new Vector3(0, 1);

        float dist = PlayerState.isStealth && !_deacsth ? _dist / 4 : _dist;

        //Debug.DrawRay(Mover.position + (Vector3)applyVector, (Vector3)applyVector * _dist, Color.red, 0.2f);
        RaycastHit2D hit = Physics2D.Raycast(Mover.position + (Vector3)applyVector, applyVector, dist, ~LayerManager.LayertoIgnore);
        if (hit.collider != null && (playerLayer & (1 << hit.collider.gameObject.layer)) != 0)
        {
            return true;
        }

        for (int i = 1; i < _maxErr + 1; i++)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                //Debug.DrawRay(Mover.position + i * j * v, (Vector3)applyVector * _dist, Color.red, 0.2f);
                hit = Physics2D.Raycast(Mover.position + i * j * v, applyVector, dist, ~LayerManager.LayertoIgnore);
                if (hit.collider != null && (playerLayer & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    return true;
                }
            }
        }
        return false;
    }


    // set direction to where player is 
    protected void SeeTarget(Vector3 _target, float _turnDelay = 0)
    {
        StartCoroutine(SeeTargetCoroutine(_target, _turnDelay));
    }
    IEnumerator SeeTargetCoroutine(Vector3 _target, float _turnDelay)
    {
        //Debug.Log(_target);
        yield return new WaitForSeconds(_turnDelay);
        if (Mathf.Abs(Mover.position.x - _target.x) >= Mathf.Abs(Mover.position.y - _target.y))
        {
            applyVector.y = 0f;
            if (Mover.position.x < _target.x)
                applyVector.x = 1f;
            else
                applyVector.x = -1f;
        }
        else
        {
            applyVector.x = 0f;
            if (Mover.position.y < _target.y)
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

    private void OnDrawGizmos()
    {

    }
}


