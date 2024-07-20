using System;
using System.Collections.Generic;
using UnityEngine;

class AttTarget
{
    public Attractive att;
    public float timer;

    public AttTarget(Attractive _att, float _timer)
    {
        att = _att;
        timer = _timer;
    }
}

public class Attractor : MonoBehaviour
{
    [SerializeField] private bool isPoint;
    [SerializeField] private float minAtt;
    [SerializeField] private Vector2 dir;
    [SerializeField] private int div = 100;
    [SerializeField] private LayerMask attractLayer;

    private List<AttTarget> targets = new();

    private void Update()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if ((!isPoint) // if ispoint, check minAtt
                || Mathf.Abs(targets[i].att.transform.position.x - this.transform.position.x) > minAtt
                || Mathf.Abs(targets[i].att.transform.position.y - this.transform.position.y) > minAtt)
            {
                if (targets[i].timer < 0)
                {
                    targets[i].att.Attract(GetDir(targets[i].att), div + targets[i].att.theStat.Mass);
                    targets[i].timer = (div + targets[i].att.theStat.Mass) * Time.deltaTime;
                }
                targets[i].timer -= Time.deltaTime;
            }
        }
    }

    private Vector2 GetDir(Attractive _att)
    {
        if (isPoint)
        {
            dir = _att.transform.position - this.transform.position;
            float absX = Mathf.Abs(dir.x),
                  absY = MathF.Abs(dir.y);
            if (absX > absY)
            {
                if (absX * 0.4142f > absY)
                {
                    dir.x /= -absX;
                    dir.y = 0;
                }
                else
                {
                    dir.x /= -absX;
                    dir.y /= -absY;
                }
            }
            else
            {
                if (absY * 0.4142f > absX)
                {
                    dir.x = 0;
                    dir.y /= -absY;
                }
                else
                {
                    dir.x /= -absX;
                    dir.y /= -absY;
                }
            }
        }
        return dir;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((attractLayer & (1 << other.gameObject.layer)) != 0 // can only attract specific layer
            && other.gameObject.TryGetComponent(out Attractive attractive))
        {
            targets.Add(new AttTarget(attractive, 0));
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        targets.Remove(targets.Find(hit => hit.att.gameObject == other.gameObject));
    }
}
