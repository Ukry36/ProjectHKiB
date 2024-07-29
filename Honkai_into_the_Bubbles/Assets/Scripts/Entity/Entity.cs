using System.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    public int ID;
    public string Name;
    public List<Color> ThemeColors = new();

    public Animator Animator { get; private set; }
    protected SpriteRenderer SpriteRenderer { get; private set; }
    [SerializeField] protected SpriteLibrary SpriteLibrary;

    public BoxCollider2D boxCollider;
    public Transform Mover;// who moves
    public MovePoint MovePoint; // destination to move
    public Transform GazePoint; // point to see
    public LayerMask wallLayer; // cannot walk through
    public LayerMask NoMovepointWallLayer { get; private set; }

    [SerializeField] public float DefaultSpeed = 4f; // default speed
    [HideInInspector] public float MoveSpeed { get; private set; } // actual speed (movespeed = default * sprint * ex)
    public float SprintCoeff = 2f;

    public Status theStat { get; private set; }
    public PathFindManager PathFinder { get; private set; }

    public int InvincibleFrame = 4; // invincible for n frame after hit

    [HideInInspector] public Transform target;

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        SpriteLibrary = GetComponent<SpriteLibrary>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        theStat = Mover.GetComponent<Status>();
        PathFinder = GetComponent<PathFindManager>();
        MoveSpeed = DefaultSpeed;
        NoMovepointWallLayer = wallLayer & ~(1 << LayerMask.NameToLayer("Movepoint"));
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    public void SetSpeedDefault()
    {
        MoveSpeed = DefaultSpeed * PlayerManager.instance.exSpeedCoeff;
    }

    public void SetSpeedSprint()
    {
        MoveSpeed = DefaultSpeed * SprintCoeff * PlayerManager.instance.exSpeedCoeff;
    }

    public Vector2 SetVectorOne(Vector2 _v)
    {
        return new Vector2
        (
            _v.x == 0 ? 0 : Mathf.Sign(_v.x),
            _v.y == 0 ? 0 : Mathf.Sign(_v.y)
        );
    }

    public Vector2 GazePointToDir4() =>
    Mathf.RoundToInt(Mathf.Atan2(GazePoint.localPosition.y, GazePoint.localPosition.x) * 2 / Mathf.PI) switch
    {
        -2 => new Vector2(-1, 0),
        -1 => new Vector2(0, -1),
        0 => new Vector2(1, 0),
        1 => new Vector2(0, 1),
        2 => new Vector2(-1, 0),
        _ => Vector2.zero
    };


    public Vector2 GazePointToDir8() =>
    Mathf.RoundToInt(Mathf.Atan2(GazePoint.localPosition.y, GazePoint.localPosition.x) * 8 / Mathf.PI) switch
    {
        -4 => new Vector2(-1, 0),
        -3 => new Vector2(-1, -1),
        -2 => new Vector2(0, -1),
        -1 => new Vector2(1, -1),
        0 => new Vector2(1, 0),
        1 => new Vector2(1, 1),
        2 => new Vector2(0, 1),
        3 => new Vector2(-1, 1),
        4 => new Vector2(-1, 0),
        _ => Vector2.zero
    };

    protected Vector3 GetRandomPos(Vector3 _origin, int _range) => new
    (
        Mathf.RoundToInt(_origin.x + Random.Range(-_range / 2, _range / 2)),
        Mathf.RoundToInt(_origin.y + Random.Range(-_range / 2, _range / 2))
    );

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

    public bool PointWallCheck(Vector3 _pos)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_pos, .4f, wallLayer);
        if (colliders != null && colliders.Length > 0)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out OneSideWall osw))
                {
                    if (osw.D && Mover.transform.position.y <= osw.transform.position.y - 0.5f
                     || osw.R && Mover.transform.position.x >= osw.transform.position.x + 0.5f
                     || osw.U && Mover.transform.position.y >= osw.transform.position.y + 0.5f
                     || osw.L && Mover.transform.position.x <= osw.transform.position.x - 0.5f)
                        return true;
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }

    public virtual void Knockback(Vector3 _attackOrigin, int _coeff)
    {
        Debug.LogError("ERROR : No Knockback Function");
    }

    public virtual void Hit(Vector3 _attackOrigin)
    {

    }


    public IEnumerator HitCoroutine()
    {
        theStat.invincible = true;
        Color color = SpriteRenderer.color;
        int t = InvincibleFrame * PlayerManager.instance.invincibleTimeCoeff;

        for (int i = 0; i < t; i++)
        {
            color.a = 0;
            SpriteRenderer.color = color;
            yield return new WaitForSeconds(0.036f);
            color.a = 1;
            SpriteRenderer.color = color;
            yield return new WaitForSeconds(0.036f);
        }

        theStat.invincible = false;
    }

    public virtual void Die()
    {
        Destroy(MovePoint.gameObject);
        Destroy(Mover.gameObject);
    }
}
