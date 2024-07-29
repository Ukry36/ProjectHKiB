using System.Collections;
using UnityEngine;

public class SupplyEvent : Event
{
    [SerializeField] private int amount;
    [SerializeField] private bool HP;
    [SerializeField] private bool GP;

    public override void StartEvent(Status _interactedEntity)
    {
        base.StartEvent(_interactedEntity);

        if (HP) _interactedEntity.HPControl(amount);
        if (GP) _interactedEntity.GPControl(amount);
        EndEvent();
    }
}
