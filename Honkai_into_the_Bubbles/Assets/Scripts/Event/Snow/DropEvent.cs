using System.Collections;
using UnityEngine;

public class DropEvent : Event
{
    [SerializeField] private Vector3 target;
    [SerializeField] private int vibrateInt;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private WaitForSeconds wait01 = new WaitForSeconds(0.01f);

    public override void StartEvent()
    {
        base.StartEvent();
        StartCoroutine(EventCoroutine());
    }

    private IEnumerator EventCoroutine()
    {
        for (int i = 0; i < vibrateInt; i++)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 5f);
            yield return wait01;
            this.transform.rotation = Quaternion.Euler(0, 0, -5f);
            yield return wait01;
        }

        this.transform.position -= 0.2f * Vector3.down;
        yield return wait01;
        this.transform.position = target;
        spriteRenderer.sortingLayerName = "BottomWall";
        EndEvent();
    }
}
