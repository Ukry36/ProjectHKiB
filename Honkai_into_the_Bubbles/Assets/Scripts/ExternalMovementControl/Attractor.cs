using System;
using System.Collections.Generic;
using UnityEngine;

class HitObject
{
    public GameObject gameObject;
    public float timer;

    public HitObject(GameObject _gameObject, float _timer)
    {
        gameObject = _gameObject;
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

    private List<HitObject> hitObjects = new();

    private void Update()
    {
        for (int i = 0; i < hitObjects.Count; i++)
        {
            if ((attractLayer & (1 << hitObjects[i].gameObject.layer)) != 0 // can only attract specific layer
                && ((!isPoint) || Mathf.Abs(hitObjects[i].gameObject.transform.position.x - this.transform.position.x) > minAtt
                               || Mathf.Abs(hitObjects[i].gameObject.transform.position.y - this.transform.position.y) > minAtt) // if ispoint, check minAtt
                && hitObjects[i].gameObject.TryGetComponent(out Attractive attractive)) // can only attract attractive objs
            {
                if ((div + attractive.attractive.Mass) * Time.deltaTime < hitObjects[i].timer)
                {
                    attractive.Attract(GetDir(attractive), div);
                    hitObjects[i].timer = 0;
                }
                hitObjects[i].timer += Time.deltaTime;
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

        hitObjects.Add(new HitObject(other.gameObject, 0));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        hitObjects.Remove(hitObjects.Find(hit => hit.gameObject == other.gameObject));
    }
}
