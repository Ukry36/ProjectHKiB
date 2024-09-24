using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KillAllTest : Event
{
    public LayerMask layerToKill;
    protected override void StartEvent(Status _interactedEntity)
    {
        StartCoroutine(Cooltime());
        Collider2D[] cols = Physics2D.OverlapCircleAll(this.transform.position, 100, layerToKill);

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].TryGetComponent(out Status a))
            {
                cols[i].gameObject.SetActive(false);
            }
        }
    }
}
