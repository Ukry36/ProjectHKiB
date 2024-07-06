using System.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation;
using NaughtyAttributes;
using System.Collections.Generic;
using System;

public class Entity : MonoBehaviour
{
    public int ID;
    public string Name;
    public List<Color> ThemeColors = new();

    public Animator Animator { get; private set; }
    protected SpriteRenderer SpriteRenderer { get; private set; }
    [SerializeField] protected SpriteLibrary SpriteLibrary;

    public Transform Mover;// who moves
    public MovePoint MovePoint; // destination to move
    public Transform GazePoint; // point to see
    public LayerMask wallLayer; // cannot walk through
    public LayerMask NoMovepointWallLayer { get; private set; }

    [SerializeField] private float DefaultSpeed = 4f; // default speed
    [HideInInspector] public float MoveSpeed { get; private set; } // actual speed (movespeed = default * sprint * ex)
    public float SprintCoeff = 2f;

    public Status theStat { get; private set; }
    public PathFindManager PathFinder { get; private set; }

    public int InvincibleFrame = 4; // invincible for n frame after hit

    [HideInInspector] public Transform target;

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
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
    Mathf.RoundToInt(Mathf.Atan2(Mover.position.y - GazePoint.position.y, Mover.position.x - GazePoint.position.x) * 2 / Mathf.PI) switch
    {
        -2 => new Vector2(1, 0),
        -1 => new Vector2(0, 1),
        0 => new Vector2(-1, 0),
        1 => new Vector2(0, -1),
        2 => new Vector2(1, 0),
        _ => Vector2.zero
    };


    public Vector2 GazePointToDir8() =>
    Mathf.RoundToInt(Mathf.Atan2(Mover.position.y - GazePoint.position.y, Mover.position.x - GazePoint.position.x) * 8 / Mathf.PI) switch
    {
        -4 => new Vector2(1, 0),
        -3 => new Vector2(1, 1),
        -2 => new Vector2(0, 1),
        -1 => new Vector2(-1, 1),
        0 => new Vector2(-1, 0),
        1 => new Vector2(-1, -1),
        2 => new Vector2(0, -1),
        3 => new Vector2(1, -1),
        4 => new Vector2(1, 0),
        _ => Vector2.zero
    };

    protected Vector3 GetRandomPos(Vector3 _origin, int _range) => new
    (
        Mathf.RoundToInt(_origin.x + UnityEngine.Random.Range(-_range / 2, _range / 2)),
        Mathf.RoundToInt(_origin.y + UnityEngine.Random.Range(-_range / 2, _range / 2))
    );

    public virtual void Knockback(Vector3 _attackOrigin, int _coeff)
    {
        Debug.LogError("ERROR : No Knockback Function");
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
