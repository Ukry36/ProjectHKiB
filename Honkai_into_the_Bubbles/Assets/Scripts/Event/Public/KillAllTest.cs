using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KillAllTest : Event
{
    protected override void StartEvent(Status _interactedEntity)
    {
        StartCoroutine(Cooltime());
        PoolManager.instance.KillAll();
    }
}
