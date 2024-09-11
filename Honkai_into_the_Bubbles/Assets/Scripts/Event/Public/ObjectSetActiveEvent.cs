using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSetActiveEvent : Event
{
    [SerializeField] private List<GameObject> ObjectsToActivate;
    [SerializeField] private List<GameObject> ObjectsToDeactivate;

    protected override void StartEvent(Status _interactedEntity)
    {
        Debug.Log("yes");
        bool deactivateSelf = false;
        for (int i = 0; i < ObjectsToActivate.Count; i++)
        {
            ObjectsToActivate[i].SetActive(true);
        }
        for (int i = 0; i < ObjectsToDeactivate.Count; i++)
        {
            if (ObjectsToDeactivate[i] == this.gameObject)
            {
                deactivateSelf = true;
                continue;
            }

            ObjectsToDeactivate[i].SetActive(false);
        }
        EndEvent();

        if (deactivateSelf)
        {
            this.gameObject.SetActive(false);
            isCooltime = false;
        }

    }
}
