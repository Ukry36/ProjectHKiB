using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBullet : AttackCollision
{
    public LayerMask destroyLayer;
    public Transform targetPos; // default is player
    [SerializeField] private float maxTurnSpeed = 1000f;
    [SerializeField] private float turnAcceleration = 40f;
    [SerializeField] private Vector3 moveWay = new(0, 24f, 0);
    [SerializeField] private float lastTime = 5f;
    [SerializeField] private float activateColliderLength = 0.5f;
    private Vector3 vectorToTarget;
    private float time = 0;
    private float turnSpeed = 0;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        prevPos = this.transform.position;
    }

    private void OnEnable()
    {
        if (targetPos != null)
            boxCollider2D.enabled = false;
        StartCoroutine(FireCoroutine());
    }

    IEnumerator FireCoroutine()
    {
        while (true)
        {
            yield return null;

            if (targetPos != null)
            {
                vectorToTarget = targetPos.position - this.transform.position;

                if (Vector3.Distance(this.transform.position, targetPos.position) <= activateColliderLength)
                    boxCollider2D.enabled = true;

                if (turnSpeed < maxTurnSpeed)
                    turnSpeed += turnAcceleration;
                LookAt(turnSpeed);
            }

            prevPos = this.transform.position;
            for (int i = 0; i < 3; i++)
                this.transform.Translate(moveWay * Time.deltaTime / 3);

            if (time > lastTime)
                Destroy(this.gameObject);

            time += Time.deltaTime;
        }
    }

    private void LookAt(float _speed)
    {
        Vector3 quaternionToTarget = Quaternion.Euler(0, 0, this.transform.rotation.z) * vectorToTarget;

        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: quaternionToTarget);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, _speed * Time.deltaTime);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (targetPos != null)
        {
            if (other.gameObject == targetPos.gameObject)
                Destroy(this.gameObject);
        }
        if ((destroyLayer & (1 << other.gameObject.layer)) != 0)
        {
            Destroy(this.gameObject);
        }
    }
}
