using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.U2D.Animation;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;


public class Enemy : Entity
{
    public Skill[] SkillArray;

    [HideInInspector] public List<Node> PathList;
    [HideInInspector] public Vector3 moveDir;

    [SerializeField] private GameObject beforeAttackEffectPrefab;
    [SerializeField] private AudioSource TinkerAudioSource;

    [BoxGroup("Move Algorythm")]
    public LayerMask targetLayer;
    [BoxGroup("Move Algorythm")]
    public float endFollowRadius = 12;
    [BoxGroup("Move Algorythm")]
    public float followRadius = 8;
    [BoxGroup("Move Algorythm")]
    public bool randomMove = true; private bool RMBoolean() => !randomMove;
    [BoxGroup("Move Algorythm")]
    [ShowIf("RMBoolean")]
    public bool moveByPathFind = true; private bool MBPFBoolean() => !moveByPathFind;
    [BoxGroup("Move Algorythm")]
    [ShowIf("moveByPathFind")]
    public List<StrictMoveNode> StrictMoveNodes;
    [BoxGroup("Move Algorythm")]
    [ShowIf("MBPFBoolean")]
    public List<StrictMoveNode> StrictMoveDirections; // doesn't use 'node' parameter

    [HideInInspector] public int strictMoveProcess = 0;
    [HideInInspector] public bool isDetectCooltime;

    protected override void Awake()
    {
        base.Awake();

    }

    public void SetMoveDirRandom()
    {
        if (Random.Range(-1f, 1f) > 0)
            moveDir.x = 0;
        else
            moveDir.x = Mathf.Sign(Random.Range(-1f, 1f));
        if (Random.Range(-1f, 1f) > 0)
            moveDir.y = 0;
        else
            moveDir.y = Mathf.Sign(Random.Range(-1f, 1f));
    }

    public IEnumerator SeeGazePoint(float _turnDelay = 0)
    {
        Vector3 dir = new();
        yield return new WaitForSeconds(_turnDelay);
        if (Mathf.Abs(Mover.position.x - GazePoint.position.x) >= Mathf.Abs(Mover.position.y - GazePoint.position.y))
        {
            dir.y = 0f;
            dir.x = Mover.position.x < GazePoint.position.x ? 1f : -1f;
        }
        else
        {
            dir.x = 0f;
            dir.y = Mover.position.y < GazePoint.position.y ? 1f : -1f;
        }
        SetAnimDir(dir);
    }


    // detect player if player is in specific area
    public Collider2D[] AreaDetectTarget(float _rad, bool _ignoreStealth = false)
    {
        float rad = PlayerManager.instance.isStealth && !_ignoreStealth ? _rad / 4 : _rad;

        return Physics2D.OverlapCircleAll(Mover.position, rad, targetLayer);
    }


    public Collider2D LineDetectTarget(float _dist, int _maxErr, bool _ignoreStealth = false)
    {
        Vector3 v = moveDir.x == 0 ? Vector3.right : Vector3.up;
        /*if (dir.x == 0)
            v = new Vector3(1, 0);
        else
            v = new Vector3(0, 1);*/

        float dist = PlayerManager.instance.isStealth && !_ignoreStealth ? _dist / 4 : _dist;

        //Debug.DrawRay(Mover.position + moveDir, moveDir * _dist, Color.red, 0.2f);
        RaycastHit2D hit = Physics2D.Raycast(Mover.position + moveDir, moveDir, dist, ~LayerManager.LayertoIgnore);
        if (hit.collider != null && (targetLayer & (1 << hit.collider.gameObject.layer)) != 0)
        {
            return hit.collider;
        }

        for (int i = 1; i < _maxErr + 1; i++)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                //Debug.DrawRay(Mover.position + i * j * v, moveDir * _dist, Color.red, 0.2f);
                hit = Physics2D.Raycast(Mover.position + i * j * v, moveDir, dist, ~LayerManager.LayertoIgnore);
                if (hit.collider != null && (targetLayer & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    return hit.collider;
                }
            }
        }
        return null;
    }

    public void SelectNearestTarget(Collider2D[] _colliders)
    {
        target = _colliders[0].transform;
        for (int i = 0; i < _colliders.Length; i++)
        {
            if (Vector3.Distance(_colliders[i].transform.position, Mover.position)
              < Vector3.Distance(target.position, Mover.position))
            {
                target = _colliders[i].transform;
            }
        }
    }

    public int SetPath()
    {
        if (target.position.x <= Mover.position.x + endFollowRadius
        && target.position.x >= Mover.position.x - endFollowRadius
        && target.position.y <= Mover.position.y + endFollowRadius
        && target.position.y >= Mover.position.y - endFollowRadius)
        {
            PathList = PathFinder.PathFinding(Mover.position - Vector3.one * endFollowRadius,
                                                Mover.position + Vector3.one * endFollowRadius,
                                                Mover.position, target.position);

            return PathList.Count;
        }
        else
        {
            return 0;
        }
    }

    // instantly go in front of player before attacking
    public void Track(Skill _skill)
    {
        Vector3 targetPos = new();
        for (int i = 0; i < PathList.Count - 1 && i < _skill.TrackingRadius; i++)
        {
            targetPos.x = PathList[i + 1].x;
            targetPos.y = PathList[i + 1].y;
            if (Physics2D.OverlapCircle(targetPos, .4f, wallLayer))
                break;
            MovePoint.transform.position = targetPos;
            Mover.position = MovePoint.transform.position;
        }
    }

    // check wall and adjust position of movepoint
    public bool MovepointAdjustCheck()
    {
        Vector3 DirX = new(moveDir.x, 0, 0);
        Vector3 DirY = new(0, moveDir.y, 0);

        if (Physics2D.OverlapCircle(MovePoint.transform.position + moveDir, .4f, 1 << LayerMask.NameToLayer("MovepointAdjust")))
        {
            if (moveDir.x == 0 || moveDir.y == 0)
            {
                return false;
            }
            else
            {
                if (Physics2D.OverlapCircle(MovePoint.transform.position + DirX, .4f, wallLayer))
                    moveDir.x = 0;

                if (Physics2D.OverlapCircle(MovePoint.transform.position + DirY, .4f, wallLayer))
                    moveDir.y = 0;

                if (Physics2D.OverlapCircle(MovePoint.transform.position + moveDir, .4f, 1 << LayerMask.NameToLayer("MovepointAdjust")))
                    return false;
            }
        }

        if (moveDir.x == 0 || moveDir.y == 0)
        {
            if (Physics2D.OverlapCircle(MovePoint.transform.position + moveDir, .4f, wallLayer))
                return true;
        }
        else // moveInput.x != 0 && moveInput.y != 0
        {
            if (Physics2D.OverlapCircle(MovePoint.transform.position + DirX, .4f, wallLayer))
                moveDir.x = 0;

            if (Physics2D.OverlapCircle(MovePoint.transform.position + DirY, .4f, wallLayer))
                moveDir.y = 0;

            if (moveDir == Vector3.zero)
                return true;

            if (moveDir.x != 0 && moveDir.y != 0)
                if (Physics2D.OverlapCircle(MovePoint.transform.position + moveDir, .4f, wallLayer))
                    MovePoint.transform.position -= DirY;
        }
        return false;
    }

    public void SetAnimDir(Vector2 _dir)
    {
        if (_dir.x != 0)
        {
            Animator.SetFloat("dirX", _dir.x);
            Animator.SetFloat("dirY", 0);
        }
        else
        {
            Animator.SetFloat("dirX", 0);
            Animator.SetFloat("dirY", _dir.y);
        }
    }


    // tinker before attack
    public void BeforeAttackTinker(Vector3 _offset)
    {
        Instantiate(beforeAttackEffectPrefab, this.transform.position + _offset, Quaternion.identity);
        TinkerAudioSource.Play();
    }

    public IEnumerator DetectCooltime()
    {
        isDetectCooltime = true;
        yield return new WaitForSeconds(1);
        isDetectCooltime = false;
    }

}
