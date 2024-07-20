using System.Collections;
using UnityEngine;

public class Attractive : MonoBehaviour
{
    private bool stopAttractboolean = true;

    [HideInInspector] public Status theStat;
    public Transform movePoint;
    [SerializeField] private LayerMask wallLayer;

    private void Awake()
    {
        theStat = GetComponent<Status>();
    }

    public void Attract(Vector2 _dir, int _div)
    {
        if (stopAttractboolean)
        {
            bool temp = false;
            Vector2 attractVector = _dir;
            if (attractVector.x == 0 || attractVector.y == 0)
            {
                if (Physics2D.OverlapCircle(movePoint.position + (Vector3)attractVector, .4f, wallLayer))
                    temp = true;
            }
            else
            {
                if (Physics2D.OverlapCircle(movePoint.position + new Vector3(attractVector.x, 0, 0), .4f, wallLayer))
                    attractVector.x = 0;

                if (Physics2D.OverlapCircle(movePoint.position + new Vector3(0, attractVector.y, 0), .4f, wallLayer))
                    attractVector.y = 0;

                if (attractVector.x == 0 && attractVector.y == 0)
                    temp = true;

                if (attractVector.x != 0 && attractVector.y != 0)
                    if (Physics2D.OverlapCircle(movePoint.position + (Vector3)attractVector, .5f, wallLayer))
                        attractVector.y = 0;
            }

            if (!temp)
                StartCoroutine(AttractCoroutine(attractVector, _div));
        }

    }

    public void DisableAttract() => stopAttractboolean = false;

    public void EnableAttract() => stopAttractboolean = true;


    private IEnumerator AttractCoroutine(Vector2 _dir, int _div)
    {
        for (int i = 0; i < _div; i++)
        {
            movePoint.position += (Vector3)_dir / _div;
            this.transform.position += (Vector3)_dir / _div;
            if (stopAttractboolean)
                yield return null;
        }
    }
}
