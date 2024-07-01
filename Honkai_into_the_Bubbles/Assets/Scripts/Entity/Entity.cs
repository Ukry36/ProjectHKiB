using System.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation;
using NaughtyAttributes;
using System.Collections.Generic;

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
    public LayerMask wallLayer; // cannot walk through

    [SerializeField] private float DefaultSpeed = 4f; // default speed
    [HideInInspector] public float MoveSpeed { get; private set; } // actual speed (movespeed = default * sprint * ex)
    public float SprintCoeff = 2f;

    public Status theStat { get; private set; }
    public BoxCollider2D BoxCollider { get; private set; }

    public int InvincibleFrame = 4; // invincible for n frame after hit
    [SerializeField] private bool explodeWhenDie = false;
    [ShowIf("explodeWhenDie")][SerializeField] protected TrackingBullet explosion;

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        theStat = Mover.GetComponent<Status>();
        BoxCollider = Mover.GetComponent<BoxCollider2D>();
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
            yield return null;
            color.a = 1;
            SpriteRenderer.color = color;
            yield return null;
        }

        theStat.invincible = false;
    }

    public void Die()
    {
        if (explodeWhenDie)
        {
            explosion.transform.parent = null;
            explosion.gameObject.SetActive(true);
        }
        Destroy(MovePoint.gameObject);
        Destroy(Mover.gameObject);
    }
}
