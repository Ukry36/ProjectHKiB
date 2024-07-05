using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Skill
{
    public LayerMask destroyLayer;
    public Transform targetPos;
    [SerializeField] private Vector3 moveWay = new(0, 24f, 0); // because its images direction could be wrong
    [SerializeField] private float maxTurnSpeed = 1000f;
    [SerializeField] private float turnAcceleration = 40f;
    [SerializeField] private float lastTime = 5f;
    [SerializeField] private float activateColliderLength = 0.5f;
    [SerializeField] private BoxCollider2D boxCollider2D;
    private float time = 0;
    private Vector3 vectorToTarget;
    private float turnSpeed = 0;


    private void OnEnable()
    {
        time = lastTime;
        this.transform.position = Vector3.zero;
    }

    private void Update()
    {
        if (targetPos != null)
        {
            vectorToTarget = targetPos.position - this.transform.position;

            if (Vector3.Distance(this.transform.position, targetPos.position) <= activateColliderLength)
                boxCollider2D.enabled = true;

            turnSpeed += turnSpeed < maxTurnSpeed ? turnAcceleration : 0;
            LookAt(turnSpeed);
        }

        this.transform.Translate(moveWay * Time.deltaTime);

        if (time > lastTime)
            Destroy(this.gameObject);

        time += Time.deltaTime;
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
