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

    [HideInInspector] public Animator Animator { get; private set; }
    protected SpriteRenderer SpriteRenderer { get; private set; }
    protected SpriteLibrary SpriteLibrary { get; private set; }

    public Transform Mover;// who moves
    public MovePoint MovePoint; // destination to move
    public LayerMask wallLayer; // cannot walk through

    [SerializeField] private float DefaultSpeed = 4f; // default speed
    [HideInInspector] public float MoveSpeed { get; private set; } // actual speed (movespeed = default * sprint * ex)
    public float SprintCoeff = 2f;

    public Status theStat;
    public BoxCollider2D BoxCollider;
    public int InvincibleFrame = 4; // invincible for n frame after hit
    public int Mass = 0;
    [SerializeField] private bool explodeWhenDie = false;
    [ShowIf("explodeWhenDie")][SerializeField] protected TrackingBullet explosion;

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteLibrary = GetComponent<SpriteLibrary>();
    }

    protected virtual void Start()
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

    public virtual void Knockback(Vector2 _dir, int _strong) { }

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
