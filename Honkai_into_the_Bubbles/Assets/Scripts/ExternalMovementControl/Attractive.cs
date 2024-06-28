using System.Collections;
using UnityEngine;

public class Attractive : MonoBehaviour
{
    private bool stopAttractboolean = true;

    public Entity attractive { get; private set; }

    private void Awake()
    {
        attractive = GetComponent<Entity>();
    }

    public void Attract(Vector2 _dir, int _div)
    {
        if (stopAttractboolean)
        {
            float coeff = 1f;
            if (this.transform.lossyScale.x == 2)
                coeff = 1.5f;
            bool temp = false;
            Vector2 attractVector = _dir;
            if (attractVector.x == 0 || attractVector.y == 0)
            {
                if (Physics2D.OverlapCircle(attractive.MovePoint.transform.position + (Vector3)attractVector * coeff, .4f, attractive.wallLayer))
                    temp = true;
            }
            else
            {
                if (Physics2D.OverlapCircle(attractive.MovePoint.transform.position + new Vector3(attractVector.x, 0, 0) * coeff, .4f, attractive.wallLayer))
                    attractVector.x = 0;

                if (Physics2D.OverlapCircle(attractive.MovePoint.transform.position + new Vector3(0, attractVector.y, 0) * coeff, .4f, attractive.wallLayer))
                    attractVector.y = 0;

                if (attractVector.x == 0 && attractVector.y == 0)
                    temp = true;

                if (attractVector.x != 0 && attractVector.y != 0)
                    if (Physics2D.OverlapCircle(attractive.MovePoint.transform.position + (Vector3)attractVector * coeff, .5f, attractive.wallLayer))
                        attractVector.y = 0;
            }

            if (!temp)
                StartCoroutine(AttractCoroutine(attractVector, _div + attractive.Mass));
        }

    }

    public void DisableAttract() => stopAttractboolean = false;

    public void EnableAttract() => stopAttractboolean = true;


    private IEnumerator AttractCoroutine(Vector2 _dir, int _div)
    {
        for (int i = 0; i < _div; i++)
        {
            attractive.MovePoint.transform.position += (Vector3)_dir / _div;
            attractive.Mover.position += (Vector3)_dir / _div;
            if (stopAttractboolean)
                yield return null;
        }
    }
}
