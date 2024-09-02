using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Collections;

public class Enemy : Entity
{
    public Skill[] SkillArray;

    [HideInInspector] public List<Node> PathList;

    [SerializeField] private GameObject beforeAttackEffectPrefab;

    [BoxGroup("Move Algorythm")]
    public LayerMask targetLayer;
    [BoxGroup("Move Algorythm")]
    public float endFollowRadius = 12;
    [BoxGroup("Move Algorythm")]
    public float followRadius = 8;
    [BoxGroup("Move Algorythm")]
    public float backstepRadius = 4;
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
    public float detectCooltime = 1f;

    [HideInInspector] public bool isTurnCooltime;
    public float turnCooltime = 1f;

    [HideInInspector] public bool backStep;


    protected override void Awake()
    {
        base.Awake();

    }

    public void SetMoveDirRandom4()
    {
        GazePoint.position = Mover.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        moveDir = GazePointToDir4();
    }

    public void SetMoveDirRandom8()
    {
        GazePoint.position = Mover.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        moveDir = GazePointToDir8();
    }

    // detect player if player is in specific area
    public Collider2D[] AreaDetectTarget(float _rad, bool _ignoreStealth = false, Vector3 offset = new Vector3())
    {
        float rad = PlayerManager.instance.isStealth && !_ignoreStealth ? _rad / 4 : _rad;
        return Physics2D.OverlapCircleAll(Mover.position + offset, rad, targetLayer);
    }


    public Collider2D LineDetectTarget(Vector2 _dir, float _dist, int _maxErr, bool _ignoreStealth = false)
    {
        Vector3 v = _dir.x == 0 ? Vector3.right : Vector3.up;
        float sizeCoefficient = theStat.Size switch { 1 => 1, 2 => 1.5f, _ => 1 };

        float dist = PlayerManager.instance.isStealth && !_ignoreStealth ? _dist / 4 : _dist;

        Debug.DrawRay(Mover.position + (Vector3)_dir * sizeCoefficient, _dir * _dist, Color.red, 0.2f);
        RaycastHit2D hit = Physics2D.Raycast(Mover.position + (Vector3)_dir * sizeCoefficient, _dir, dist, ~LayerManager.instance.ignoreRaycast);
        if (hit.collider != null && (targetLayer & (1 << hit.collider.gameObject.layer)) != 0)
        {
            return hit.collider;
        }

        for (int i = 1; i < _maxErr * 2 + 1; i++)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                Debug.DrawRay(Mover.position + i * j * v * 0.5f + (Vector3)_dir * sizeCoefficient, _dir * _dist, Color.red, 0.2f);
                hit = Physics2D.Raycast(Mover.position + i * j * v * 0.5f + (Vector3)_dir * sizeCoefficient, _dir, dist, ~LayerManager.instance.ignoreRaycast);
                if (hit && hit.collider != null && (targetLayer & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    return hit.collider;
                }
            }
        }
        return null;
    }

    public IEnumerator DetectCooltime()
    {
        isDetectCooltime = true;
        yield return new WaitForSeconds(detectCooltime);
        isDetectCooltime = false;
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

    public void SelectFarthestTarget(Collider2D[] _colliders)
    {
        target = _colliders[0].transform;
        for (int i = 0; i < _colliders.Length; i++)
        {
            if (Vector3.Distance(_colliders[i].transform.position, Mover.position)
              > Vector3.Distance(target.position, Mover.position))
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

    // tinker before attack
    public void BeforeAttackTinker(Vector3 _offset)
    {
        PoolManager.instance.ReuseGameObject(beforeAttackEffectPrefab, this.transform.position + _offset, Quaternion.identity);
    }

    public IEnumerator TurnCooltime()
    {
        isTurnCooltime = true;
        yield return new WaitForSeconds(turnCooltime);
        yield return null;
        isTurnCooltime = false;
    }

    public IEnumerator BackStep(float _time)
    {
        backStep = true;
        yield return new WaitForSeconds(_time);
        backStep = false;
    }
}
