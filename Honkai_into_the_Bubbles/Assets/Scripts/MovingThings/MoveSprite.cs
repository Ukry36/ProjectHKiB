using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.U2D.Animation;
using NaughtyAttributes;

public class MoveSprite : MonoBehaviour
{
    public int ID;
    public string characterName;
    public Color themeColor1;
    public Color themeColor2;
    public BoxCollider2D boxCollider;
    public LayerMask wallLayer; // recognz as wall
    public float defaultSpeed = 4f; // orig speed
    protected float moveSpeed; // apply speed
    public Transform Mover; // who moves
    public Transform movePoint; // destination
    protected bool walking = false; // if true, WalkCoroutine is running
    protected bool grrogying = false; // also used as superarmor!!!!!
    protected Vector2 applyVector; // apply while walking
    protected Queue<string> queue; // used for sequential movement
    public bool freeze = false;
    [SerializeField] private int grrogyCoefficient = 3; // minimum 2
    protected bool stopWalkboolean = true;
    protected bool stopAttackboolean = true;
    [HideInInspector] public Animator animator; // animator
    protected SpriteRenderer spriteRenderer; // spriteRenderer
    [SerializeField] protected SpriteLibrary spriteLibrary;

    [SerializeField] protected bool explodeWhenDie = false;
    [ShowIf("explodeWhenDie")][SerializeField] protected TrackingBullet explosion;



    // walk by order
    public void Move(string _dir, float _wait = 0, bool _sprint = false)
    {
        queue.Enqueue(_dir);
        StartCoroutine(MoveCoroutine(_dir, _wait, _sprint));
    }


    IEnumerator MoveCoroutine(string _dir, float _wait, bool _sprint)
    {
        walking = true;
        while (queue.Count != 0 && stopWalkboolean)
        {
            // set movespeed default
            // if _sprint is true, movespeed doubles
            if (_sprint)
                moveSpeed = defaultSpeed * 2;
            else
                moveSpeed = defaultSpeed;


            // wait before movin
            if (_wait > 0)
                yield return new WaitForSeconds(_wait);


            // dequeue desired direction
            string direction = queue.Dequeue();


            // apply directrion to anim and vector
            applyVector.Set(0, 0);
            switch (direction)
            {
                case "UP":
                    applyVector.y = 1f;
                    break;
                case "DOWN":
                    applyVector.y = -1f;
                    break;
                case "RIGHT":
                    applyVector.x = 1f;
                    break;
                case "LEFT":
                    applyVector.x = -1f;
                    break;
            }
            animator.SetFloat("DirX", applyVector.x);
            animator.SetFloat("DirY", applyVector.y);


            // if there is a wall on destination, quit moving on current direction
            if (Physics2D.OverlapCircle(movePoint.position + (Vector3)applyVector, .4f, wallLayer))
            {
                animator.SetBool("walk", false);
                continue;
            }


            // move destination forward
            movePoint.position += (Vector3)applyVector;


            // move toward destination
            animator.SetBool("walk", true);
            while (Vector3.Distance(Mover.position, movePoint.position) >= .05f)
            {
                if (!stopWalkboolean)
                    break;
                Mover.position = Vector3.MoveTowards(Mover.position, movePoint.position, moveSpeed * Time.deltaTime);
                yield return null;
            }
            Mover.position = movePoint.position;

        }
        animator.SetBool("walk", false);
        walking = false;
        stopWalkboolean = true;
    }


    // exist to deactivate WalkCoroutine from children script
    public void StopWalk()
    {
        stopWalkboolean = false;
    }

    // exist to deactivate AttackCoroutine from children script
    public void StopAttack()
    {
        stopAttackboolean = false;
    }


    public void Grrogy(Vector3 _dir)
    {
        if (!grrogying)
        {
            grrogying = true;
            StopWalk();
            StopAttack();
            StartCoroutine(GrrogyCoroutine(_dir));
        }
    }


    IEnumerator GrrogyCoroutine(Vector3 _dir)
    {
        yield return null;
        yield return null;
        yield return null;
        if (_dir.x != 0)
        {
            animator.SetFloat("dirX", -_dir.x);
            animator.SetFloat("dirY", 0);
        }
        else
        {
            animator.SetFloat("dirX", 0);
            animator.SetFloat("dirY", -_dir.y);
        }

        for (int i = 0; i < grrogyCoefficient - 1; i++)
        {
            Mover.position = movePoint.position;


            if (_dir.x == 0 || _dir.y == 0)
            {
                if (Physics2D.OverlapCircle(movePoint.position + new Vector3(_dir.x, _dir.y, 0), .4f, wallLayer))
                    _dir = Vector3.zero;
            }
            else
            {
                if (Physics2D.OverlapCircle(movePoint.position + new Vector3(_dir.x, 0, 0), .4f, wallLayer))
                {
                    _dir.x = 0;
                    animator.SetFloat("dirX", 0);
                    animator.SetFloat("dirY", -_dir.y);
                }


                if (Physics2D.OverlapCircle(movePoint.position + new Vector3(0, _dir.y, 0), .4f, wallLayer))
                    _dir.y = 0;


                if (_dir.x != 0 && _dir.y != 0)
                    _dir.y = 0;
            }

            movePoint.position += _dir;
        }


        while (Vector3.Distance(Mover.position, movePoint.position) >= .05f)
        {
            Mover.position = Vector3.MoveTowards(Mover.position, movePoint.position, 4 * Time.deltaTime);
            yield return null;
        }
        Mover.position = movePoint.position;

        yield return new WaitForSeconds(0.2f);
        grrogying = false;
    }

    public void Hit()
    {
        StartCoroutine(HitCoroutine());
    }

    IEnumerator HitCoroutine()
    {
        Color color = spriteRenderer.color;

        color.g = 0;
        color.b = 0;
        spriteRenderer.color = color;

        yield return new WaitForSeconds(0.2f);

        color.g = 225;
        color.b = 225;
        spriteRenderer.color = color;

    }

    public void Die()
    {
        if (explodeWhenDie)
        {
            explosion.transform.parent = null;
            explosion.gameObject.SetActive(true);
        }
        Destroy(movePoint.gameObject);
        Destroy(Mover.gameObject);
    }


}
