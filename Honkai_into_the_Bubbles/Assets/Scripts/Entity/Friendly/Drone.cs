using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Drone : Friendly
{
    private WaitForSeconds wait05 = new(0.5f);
    private WaitForSeconds wait005 = new(0.05f);

    [SerializeField] private GameObject bulletPrefab;

    private bool isFiring = false;
    private float targetTimer;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        Collider2D enemyCollider = Physics2D.OverlapCircle(Mover.position, followRadius, targetLayer);

        // Set Target
        if (targetTimer < 0)
        {
            if (Vector3.Distance(Mover.position, Player.position) < endFollowRadius && enemyCollider != null)
            {
                target = enemyCollider.transform;
            }
            else
            {
                target = Player;
            }

            targetTimer = 2f;
        }
        if (target == null)
            target = Player;
        targetTimer -= Time.deltaTime;

        //Firing Area
        if (target != Player && !isFiring)
        {
            StartCoroutine(FiringCoroutine(enemyCollider.transform.position));
        }

        //Moving Area
        MovePoint.transform.position = target.position;
        GazePoint.position = target.position;

        Vector3 direction = MovePoint.transform.position - Mover.position;

        SetAnimDir(GazePointToDir4());

        if (Vector3.Distance(Mover.position, MovePoint.transform.position) > 3f)
            Mover.position = Vector3.Lerp(Mover.position, MovePoint.transform.position - direction.normalized * 3, MoveSpeed * Time.deltaTime);

    }



    private IEnumerator FiringCoroutine(Vector3 _targetPos)
    {
        isFiring = true;
        Vector3 direction = _targetPos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        for (int i = 0; i < 5; i++)
        {
            var clone = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));
            clone.SetActive(true);
            yield return wait005;
        }

        yield return wait05;
        isFiring = false;
    }
}