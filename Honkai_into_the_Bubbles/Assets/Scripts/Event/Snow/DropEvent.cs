using System.Collections;
using UnityEngine;

public class DropEvent : Event
{
    [SerializeField] private float mass = 1;
    [SerializeField] private Transform targetToMove;
    [SerializeField] private int vibrateInt;

    private WaitForSeconds wait01 = new(0.02f);

    protected override void StartEvent(Status _interactedEntity)
    {
        StartCoroutine(Cooltime());
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
        float time = 0;
        while (this.transform.position.y > targetToMove.position.y)
        {
            this.transform.Translate(Vector3.down * 9.8f * mass * time);
            time += Time.deltaTime;
            yield return null;
        }
        this.transform.rotation = Quaternion.identity;
        EndEvent();
    }
}
