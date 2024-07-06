using System.Collections;
using UnityEngine;

public class Drone : MoveViaAlgorithm
{
    private WaitForSeconds wait = new(0.5f);

    [SerializeField] private GameObject bulletPrefab;

    private bool isFiring = false;
    private bool isChasing = false;


    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        animator = GetComponent<Animator>();
        pathFinder = GetComponentInChildren<PathFindManager>();
        Player = FindObjectOfType<PlayerManager>().transform;
        PlayerState = Player.GetComponent<Status>();
        moveSpeed = defaultSpeed;

        StartCoroutine(DefaultCoroutine());
    }

    private IEnumerator DefaultCoroutine()
    {
        float chasingTime = 0f;
        Transform targetTrasnform = Player.transform;

        while (true)
        {
            Collider2D enemyCollider = Physics2D.OverlapCircle(Mover.position, 5f, enemyLayer);

            if (isChasing)
            {
                chasingTime += Time.deltaTime;

                if (chasingTime > 3f) { isChasing = false; chasingTime = 0f; }
            }
            else
            {
                isChasing = true;
                targetTrasnform = Player.transform;
                if (Vector3.Distance(Mover.position, Player.position) < 5f && enemyCollider != null)
                {
                    targetTrasnform = enemyCollider.transform;
                }
            }

            //Firing Area
            if (enemyCollider != null && !isFiring)
            {
                StartCoroutine(FiringCoroutine(enemyCollider.transform.position));
            }

            //Moving Area

            movePoint.position = targetTrasnform.position;

            Vector3 direction = movePoint.position - Mover.position;
            SeeTarget(targetTrasnform.position);

            if (Vector3.Distance(Mover.position, movePoint.position) > 3f)
                Mover.position = Vector3.Lerp(Mover.position, movePoint.position - direction.normalized * 3, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator FiringCoroutine(Vector3 _targetPos)
    {
        isFiring = true;
        Vector3 direction = _targetPos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        var clone = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));

        clone.SetActive(true);
        yield return wait;
        isFiring = false;
    }
}