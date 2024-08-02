using System.Collections;
using UnityEngine;

public class Attractive : MonoBehaviour
{
    private bool stopAttract;
    private bool endAttract;

    [HideInInspector] public Status theStat;
    public Transform movePoint;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private int size = 1;

    private void Awake()
    {
        theStat = GetComponent<Status>();
    }

    public void Attract(Vector3 _dir, int _div)
    {
        if (!stopAttract)
        {
            Vector3 DirX = new(_dir.x, 0, 0);
            Vector3 DirY = new(0, _dir.y, 0);

            bool stop = false;
            float wallCheckCoeff = size switch { 1 => 1, 2 => 1.5f, _ => 1 };

            if (_dir.x == 0 || _dir.y == 0)
            {
                if (PointWallCheck(movePoint.position + _dir * wallCheckCoeff))
                    stop = true;
            }
            else
            {
                if (PointWallCheck(movePoint.position + DirX * wallCheckCoeff))
                    _dir.x = 0;

                if (PointWallCheck(movePoint.position + DirY * wallCheckCoeff))
                    _dir.y = 0;

                if (_dir == Vector3.zero)
                    stop = true;

                if (_dir.x != 0 && _dir.y != 0)
                    if (PointWallCheck(movePoint.position + _dir * wallCheckCoeff))
                        _dir.y = 0;
            }

            if (!stop)
                StartCoroutine(AttractCoroutine(_dir, _div));
        }
    }

    public void DisableAttract()
    {
        stopAttract = true;
        EndAttract();
    }

    public void EnableAttract() => stopAttract = false;

    public void EndAttract() => endAttract = true;


    private IEnumerator AttractCoroutine(Vector2 _dir, int _div)
    {
        for (int i = 0; i < _div; i++)
        {
            movePoint.position += (Vector3)_dir / _div;

            if (!endAttract)
            {
                this.transform.position += (Vector3)_dir / _div;
                yield return null;
            }

        }
        endAttract = false;
    }

    private bool PointWallCheck(Vector3 _pos)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_pos, .4f, wallLayer);
        if (colliders != null && colliders.Length > 0)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out OneSideWall osw))
                {
                    if (osw.D && this.transform.position.y <= osw.transform.position.y - 0.5f
                     || osw.R && this.transform.position.x >= osw.transform.position.x + 0.5f
                     || osw.U && this.transform.position.y >= osw.transform.position.y + 0.5f
                     || osw.L && this.transform.position.x <= osw.transform.position.x - 0.5f)
                        return true;
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }
}
