using System.Collections;
using UnityEngine;

public class SupplyEvent : Event
{
    [SerializeField] private int amount;
    [SerializeField] private bool HP;
    [SerializeField] private bool GP;
    [SerializeField] private bool setRaw;

    protected override void StartEvent(Status _interactedEntity)
    {
        StartCoroutine(Cooltime());
        if (!setRaw)
        {
            if (HP) _interactedEntity.HPControl(amount);
            if (GP) _interactedEntity.GPControl(amount);
        }
        else
        {
            if (HP) _interactedEntity.HPControl(amount - _interactedEntity.CurrentHP);
            if (GP) _interactedEntity.GPControl(amount - _interactedEntity.CurrentGP);
        }

        EndEvent();

    }
}
