using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class Enemy_Lightning1 : MoveViaAlgorithm
{
    private WaitForSeconds wait = new(0.5f);
    private Vector3 tinkerOffset = new(0, .5f, 0);
    [SerializeField] private GameObject trackingBulletPrefab;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject targetPrefab;
    [HideInInspector] public bool fire = false;
    private Vector3 attackPos;

    private Vector3 ofs1 = Vector3.zero;
    private Vector3 ofs2 = Vector3.zero;
    private Quaternion dir = Quaternion.identity;

    // Start is called before the first frame update
    private void Start()
    {

        movePoint.parent = null;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Player = FindObjectOfType<PlayerManager>().transform;
        PlayerState = Player.GetComponent<Status>();
        pathFinder = GetComponent<PathFindManager>();
        moveSpeed = defaultSpeed;


        /*
        SkillArray = new Skill[] {
            new Skill(15, 0, 4, 3.5f, 0.2f, true),
            new Skill(30, 0, 8, 3f, 0.2f, false)
        }; //upper skill high priority (not nessesory just for easy recognizing)
        *//*


        bulletPrefab.GetComponent<AttackCollision>().
        SetAttackInfo(SkillArray[0].DamageCoefficient, SkillArray[0].BaseCriticalRate, SkillArray[0].Strong > 0);
        trackingBulletPrefab.GetComponent<AttackCollision>().
        SetAttackInfo(SkillArray[1].DamageCoefficient, SkillArray[1].BaseCriticalRate, SkillArray[1].Strong > 0);

        theState.SetHitAnimObject();

        StartCoroutine(NormalBehaviourCoroutine());
    }

    IEnumerator AggroBehaviourCoroutine()
    {
        while (true)
        {
            while (grrogying)
                yield return wait;


            if (!DetectPlayer(endFollowRadius))
            {
                StartCoroutine(NormalBehaviourCoroutine());
                break;
            }


            SeeTarget(Player.position, turnDelay);
            yield return new WaitForSeconds(turnDelay);


            if (DetectPlayer(SkillArray[1].DetectRadius, true))
            {
                StartCoroutine(Skill02Coroutine());
                break;
            }
            else if (DetectPlayer(SkillArray[0].DetectRadius, true))
            {
                StartCoroutine(Skill01Coroutine());
                break;
            }


            yield return new WaitForSeconds(AggroMoveDelay);
            if (DetectPlayer(followRadius / 2f))
            {
                walking = true;
                // if there is a wall, wait
                if (Physics2D.OverlapCircle(movePoint.position - new Vector3(applyVector.x, applyVector.y, 0f) * 1.5f, .4f, wallLayer))
                {
                    yield return wait;
                    walking = false;
                    continue;
                }

                // move destination forward
                movePoint.position -= new Vector3(applyVector.x, applyVector.y, 0);
                // move toward destination
                // if stopWalkboolean is false, skip sequencial movement
                animator.SetBool("walk", true);
                while (Vector3.Distance(Mover.position, movePoint.position) >= .05f)
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
    }


    IEnumerator NormalBehaviourCoroutine()
    {
        while (true)
        {
            while (grrogying)
                yield return wait;


            if (doAttack && DetectPlayer(followRadius))
            {
                yield return null;
                StartCoroutine(AggroBehaviourCoroutine());
                break;
            }
            //ToPlayerList.Clear();


            yield return new WaitForSeconds(turnDelay);
            RandomDirection();
            yield return new WaitForSeconds(NormalMoveDelay);


            walking = true;
            // if there is a wall, wait
            if (Physics2D.OverlapCircle(movePoint.position + new Vector3(applyVector.x, applyVector.y, 0f) * 1.5f, .4f, wallLayer))
            {
                yield return wait;
                continue;
            }


            // move destination forward
            movePoint.position += new Vector3(applyVector.x, applyVector.y, 0f);
            // move toward destination
            // if stopWalkboolean is false, skip sequencial movement
            animator.SetBool("walk", true);
            while (Vector3.Distance(Mover.position, movePoint.position) >= .05f)
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


    private void FireBullet(GameObject _bullet, bool _isTracking, int _muzzle)
    {
        SetBulletTransform(_isTracking, _muzzle);


        var clone = Instantiate(_bullet, this.transform.position + ofs1, dir);
        if (_isTracking)
        {
            TrackingBullet theTB = clone.GetComponent<TrackingBullet>();
            theTB.targetPos = Instantiate(targetPrefab,
            attackPos + new Vector3((int)Random.Range(-3f, 3f), (int)Random.Range(-3f, 3f), 0),
            Quaternion.identity).transform;
        }

        clone.SetActive(true);


        clone = Instantiate(_bullet, this.transform.position + ofs2, dir);
        if (_isTracking)
        {
            TrackingBullet theTB = clone.GetComponent<TrackingBullet>();
            theTB.targetPos = Instantiate(targetPrefab,
            attackPos + new Vector3((int)Random.Range(-3f, 3f), (int)Random.Range(-3f, 3f), 0),
            Quaternion.identity).transform;
        }

        clone.SetActive(true);
    }


    private void SetBulletTransform(bool _isTracking, int _muzzle)
    {
        if (applyVector.x == 0 && applyVector.y == -1f) // D
        {
            dir = Quaternion.Euler(0, 0, 180);
            switch (_muzzle)
            {
                case 0:
                    ofs1 = new Vector3(1.2f, 0.9f, 0);
                    ofs2 = new Vector3(-1.2f, 0.9f, 0); break;
                case 1:
                    ofs1 = new Vector3(0.8f, 0.95f, 0);
                    ofs2 = new Vector3(-0.8f, 0.95f, 0); break;
                case 2:
                    ofs1 = new Vector3(1f, 0.35f, 0);
                    ofs2 = new Vector3(-1f, 0.35f, 0); break;
                default: break;
            }
        }
        if (applyVector.x == 1f && applyVector.y == 0) // R
        {
            if (_isTracking) dir = Quaternion.Euler(0, 0, -70);
            else dir = Quaternion.Euler(0, 0, -90);
            switch (_muzzle)
            {
                case 0:
                    ofs1 = new Vector3(0, 1.9f, 0);
                    ofs2 = new Vector3(0, -0.1f, 0); break;
                case 1:
                    ofs1 = new Vector3(0, 1.6f, 0);
                    ofs2 = new Vector3(0, 0.4f, 0); break;
                case 2:
                    ofs1 = new Vector3(0, 1.6f, 0);
                    ofs2 = new Vector3(0, -0.2f, 0); break;
                default: break;
            }
        }
        if (applyVector.x == 0 && applyVector.y == 1f) // U
        {
            dir = Quaternion.identity;
            switch (_muzzle)
            {
                case 0:
                    ofs1 = new Vector3(1.2f, 1.4f, 0);
                    ofs2 = new Vector3(-1.2f, 1.4f, 0); break;
                case 1:
                    ofs1 = new Vector3(0.8f, 1.25f, 0);
                    ofs2 = new Vector3(-0.8f, 1.25f, 0); break;
                case 2:
                    ofs1 = new Vector3(1f, 1f, 0);
                    ofs2 = new Vector3(-1f, 1f, 0); break;
                default: break;
            }
        }
        if (applyVector.x == -1 && applyVector.y == 0f) // L
        {
            if (_isTracking) dir = Quaternion.Euler(0, 0, 70);
            else dir = Quaternion.Euler(0, 0, 90);
            switch (_muzzle)
            {
                case 0:
                    ofs1 = new Vector3(0, 1.9f, 0);
                    ofs2 = new Vector3(0, -0.1f, 0); break;
                case 1:
                    ofs1 = new Vector3(0, 1.6f, 0);
                    ofs2 = new Vector3(0, 0.4f, 0); break;
                case 2:
                    ofs1 = new Vector3(0, 1.6f, 0);
                    ofs2 = new Vector3(0, -0.2f, 0); break;
                default: break;
            }
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
        while (grrogying)
            yield return wait;
        if (!stopAttackboolean)
        {
            StopAttacking();
            yield break;
        }


        SeeTarget(Player.position, turnDelay);
        yield return new WaitForSeconds(turnDelay);
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
        BeforeAttackTinker(tinkerOffset);
        yield return new WaitForSeconds(SkillArray[0].Delay);


        //SkillArray[0].CanSkill = false;
        animator.SetTrigger("fire");
        yield return null;


        FireBullet(bulletPrefab, false, 0);
        FireBullet(bulletPrefab, false, 2);

        yield return new WaitForSeconds(0.3f);
        FireBullet(bulletPrefab, false, 0);
        FireBullet(bulletPrefab, false, 2);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f);
        StopAttacking();


        yield return new WaitForSeconds(SkillArray[0].Cooltime);
        //SkillArray[0].CanSkill = true;
    }


    IEnumerator Skill02Coroutine()
    {
        while (grrogying)
            yield return wait;
        if (!stopAttackboolean)
        {
            StopAttacking();
            yield break;
        }


        SeeTarget(Player.position, turnDelay);
        yield return new WaitForSeconds(turnDelay);
        animator.SetFloat("skill", 2f);

        attackPos = Vector3Int.FloorToInt(Player.position);


        for (float time = 0; time < attackAnimDelay - SkillArray[1].Delay; time += Time.deltaTime)
        {
            if (!stopAttackboolean)
            {
                StopAttacking();
                yield break;
            }
            yield return null;
        }
        BeforeAttackTinker(tinkerOffset);
        yield return new WaitForSeconds(SkillArray[1].Delay);


        //SkillArray[1].CanSkill = false;
        animator.SetTrigger("fire");
        yield return null;


        for (int i = 0; i < 6; i++)
        {
            yield return new WaitForSeconds(0.2f);
            FireBullet(trackingBulletPrefab, true, i / 3);
        }
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f);
        StopAttacking();


        yield return new WaitForSeconds(SkillArray[1].Cooltime);
        //SkillArray[1].CanSkill = true;
    }
}
*/