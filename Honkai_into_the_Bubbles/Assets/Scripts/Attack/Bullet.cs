using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Skill
{
    public LayerMask destroyLayer;
    [SerializeField] private Vector3 moveWay = new(0, 24f, 0); // because its images direction could be wrong
    [SerializeField] private float lastTime = 5f;
    private float time = 0;


    protected override void OnEnable()
    {
        base.OnEnable();
        time = lastTime;
    }

    private void Update()
    {
        this.transform.Translate(moveWay * Time.deltaTime);
        if (time < 0)
        {
            this.gameObject.SetActive(false);
        }
        time -= Time.deltaTime;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if ((destroyLayer & (1 << other.gameObject.layer)) != 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
