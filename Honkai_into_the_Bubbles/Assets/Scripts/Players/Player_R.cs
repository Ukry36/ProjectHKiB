using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Player_R : MoveViaInput
{
    private float lastedTime;
    private float prevTime;
    private bool skill01ing = false;
    private bool skill02ing = false;
    private bool startAtCombo4;
    private bool keepSkill01;
    [SerializeField] private int repeatSkill03;
    [SerializeField] private GameObject SAPrefab;
    [SerializeField] private GameObject SAPrefabDiag;
    [SerializeField] private GameObject SAPrefabfor03;
    [SerializeField] private GameObject SAPrefabfor03Diag;
    [SerializeField] private GameObject AttractorPrefab;

    private int RSACI = 0;


    private void Awake()
    {
        movePoint.parent = null;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.SetFloat("dirX", 0);
        animator.SetFloat("dirY", -1);


        AttackArray = new Attack[] {
            new Attack(75, 0, 10, false),
            new Attack(50, 1, 10, true),
            new Attack(50, 1, 10, false),
            new Attack(175, 0, 10, true)
        };

        SAPrefab.GetComponent<AttackCollision>().
        SetAttackInfo(GS.skillList[2].DamageCoefficient, GS.skillList[2].CriticalRate, GS.skillList[2].Strong);
        SAPrefabDiag.GetComponent<AttackCollision>().
        SetAttackInfo(GS.skillList[2].DamageCoefficient, GS.skillList[2].CriticalRate, GS.skillList[2].Strong);
        SAPrefabfor03.GetComponent<AttackCollision>().
        SetAttackInfo(GS.skillList[2].DamageCoefficient * 5, GS.skillList[2].CriticalRate, GS.skillList[2].Strong);
        SAPrefabfor03Diag.GetComponent<AttackCollision>().
        SetAttackInfo(GS.skillList[2].DamageCoefficient * 5, GS.skillList[2].CriticalRate, GS.skillList[2].Strong);

        spriteOverrideID = ID;
    }


    private void Update()
    {
        // read vector2 from input sys
        moveInput = InputManager.instance.MoveInput;

        // if no WalkCoroutine is running and input exists, start walking
        if (!walking && moveInput != Vector2.zero && !freeze && !attacking && !dodging && !grrogying && !graffiting)
        {
            StartCoroutine(WalkCoroutine());
        }


        // recieve attack input if its right timing
        attackInput = false;
        if (recieveAttackInput)
            attackInput = InputManager.instance.AttackInput;

        // if no AttackCoroutine is running and input exists, start attacking
        if (!attacking && attackInput && !freeze && !dodging && !grrogying && !graffiting && !skill01ing)
        {
            combo = 1;
            StopWalk();
            StartCoroutine(AttackCoroutine(AttackArray[combo - 1]));
        }


        // recieve dodge input if its right timing
        dodgeInput = false;
        if (recieveDodgeInput)
            dodgeInput = InputManager.instance.DodgeInput;

        // if no AttackCoroutine is running and input exists, start attacking
        if (!dodging && dodgeInput && !freeze && !grrogying && !graffiting)
        {
            StopWalk();
            StopAttack();
            recieveAttackInput = false;
            canAttackAnim = false;
            StartCoroutine(DodgeCoroutine());
        }


        // recieve graffiti input if its right timing
        startGraffitiInput = false;
        if (recieveGraffitiInput)
            startGraffitiInput = InputManager.instance.GraffitiStartInput;
        if (!graffiting && startGraffitiInput && !freeze && !grrogying && !dodging && theStat.currentGP > 0)
        {
            StopWalk();
            StopAttack();
            recieveAttackInput = false;
            canAttackAnim = false;
            StartCoroutine(GraffitiCoroutine());
        }
    }

    public override void ChangeThemeColor(Color _c1, Color _c2)
    {
        dodgeColors.Clear();
        dodgeColors.Add(_c1);
        dodgeColors.Add(_c2);
        SAPrefab.GetComponent<SpriteRenderer>().color = _c1;
        SAPrefabDiag.GetComponent<SpriteRenderer>().color = _c2;
        SAPrefabfor03.GetComponent<SpriteRenderer>().color = _c1;
        SAPrefabfor03Diag.GetComponent<SpriteRenderer>().color = _c2;
    }

    protected override void StopAttacking()
    {
        base.StopAttacking();
        if (startAtCombo4)
        {
            animator.SetBool("skill", false);
            startAtCombo4 = false;
        }
    }


    protected override void StartSkill(int _skillNum)
    {
        if (_skillNum == 0)
        {
            skill01ing = true;
            lastedTime = 0;
            prevTime = 0;
            StartCoroutine(Skill01Coroutine(GS.skillList[0]));
        }
        else if (_skillNum == 1)
        {
            if (skill01ing || skill02ing)
            {
                skill02ing = true;
                skill01ing = false;
                lastedTime = 0;
                prevTime = 0;
                StartCoroutine(Skill01Coroutine(GS.skillList[1]));
            }
            theStat.GPControl(5);
        }
        else if (_skillNum == 2)
        {
            if (skill01ing)
            {
                StartCoroutine(Skill01Coroutine(GS.skillList[0]));
                StartCoroutine(Skill03p01Coroutine(GS.skillList[2]));
            }
            else if (skill02ing)
            {
                StartCoroutine(Skill01Coroutine(GS.skillList[1]));
                StartCoroutine(Skill03p01Coroutine(GS.skillList[2]));
            }
            else
            {
                StartCoroutine(Skill03Coroutine());
            }
        }
        else
        {
            animator.SetBool("skill", false);
            skill01ing = false;
        }
    }

    private IEnumerator Skill01Coroutine(Skill _skill)
    {
        AttractorPrefab.SetActive(true);
        attack.SetAttackInfo(_skill.DamageCoefficient, _skill.CriticalRate, _skill.Strong);
        defaultSpeed /= 2;
        animator.SetBool("skill", true);
        while (!dodgeInput && lastedTime <= _skill.Cooltime)
        {
            yield return null;
            lastedTime += Time.deltaTime;
            if (attackInput)
            {
                startAtCombo4 = true;
                break;
            }
            if (startGraffitiInput)
            {
                keepSkill01 = true;
                break;
            }
        }
        animator.SetBool("skill", false);
        if (startAtCombo4)
        {
            startAtCombo4 = false;
            stopAttackboolean = true;
            combo = 4;
            StopWalk();
            StartCoroutine(AttackCoroutine(AttackArray[combo - 1]));
        }

        if (!keepSkill01)
        { skill01ing = false; skill02ing = false; }
        else
        { keepSkill01 = false; animator.SetBool("skill", true); }
        AttractorPrefab.SetActive(false);
        defaultSpeed *= 2;
    }

    private IEnumerator Skill03p01Coroutine(Skill _skill)
    {
        prevTime = lastedTime;
        while (skill01ing || skill02ing)
        {
            yield return null;
            if (prevTime + _skill.Delay < lastedTime)
            {
                FireSA03p01();
                prevTime = lastedTime;
            }
        }
    }

    private void FireSA03p01()
    {
        if (RSACI % 2 == 0)
        {
            var clone = Instantiate(SAPrefab, this.transform.position, Quaternion.identity);
            clone.SetActive(true);
            clone = Instantiate(SAPrefab, this.transform.position, Quaternion.Euler(0, 0, 90));
            clone.SetActive(true);
            clone = Instantiate(SAPrefab, this.transform.position, Quaternion.Euler(0, 0, 180));
            clone.SetActive(true);
            clone = Instantiate(SAPrefab, this.transform.position, Quaternion.Euler(0, 0, 270));
            clone.SetActive(true);
        }
        else
        {
            var clone = Instantiate(SAPrefabDiag, this.transform.position, Quaternion.Euler(0, 0, 45));
            clone.SetActive(true);
            clone = Instantiate(SAPrefabDiag, this.transform.position, Quaternion.Euler(0, 0, 135));
            clone.SetActive(true);
            clone = Instantiate(SAPrefabDiag, this.transform.position, Quaternion.Euler(0, 0, 225));
            clone.SetActive(true);
            clone = Instantiate(SAPrefabDiag, this.transform.position, Quaternion.Euler(0, 0, 315));
            clone.SetActive(true);
        }
        RSACI++;
    }

    private IEnumerator Skill03Coroutine()
    {
        yield return new WaitUntil(() => !graffiting);
        grrogying = true;
        stopAttackboolean = true;
        combo = 3;
        StopWalk();
        StartCoroutine(AttackCoroutine(AttackArray[combo - 1]));
        for (int i = 0; i < repeatSkill03; i++)
        {
            yield return new WaitForSeconds(0.4f / repeatSkill03);
            FireSA03();
        }
        yield return new WaitUntil(() => combo != 3);
        grrogying = false;
    }

    private void FireSA03()
    {
        if (RSACI % 2 == 0)
        {
            //Vector3 quaternionToTarget = Quaternion.Euler(0, 0, this.transform.rotation.z) * applyVector;
            var clone = Instantiate(SAPrefabfor03, this.transform.position, Quaternion.LookRotation(forward: Vector3.forward, upwards: applyVector));
            clone.SetActive(true);
        }
        else
        {
            var clone = Instantiate(SAPrefabfor03Diag, this.transform.position, Quaternion.LookRotation(forward: Vector3.forward, upwards: applyVector));
            clone.SetActive(true);
        }
        RSACI++;
    }

}