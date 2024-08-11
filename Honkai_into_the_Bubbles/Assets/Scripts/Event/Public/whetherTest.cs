using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whetherTest : Event
{
    public Whether.WhetherType whetherType;
    public override void StartEvent(Status _interactedEntity)
    {
        base.StartEvent(_interactedEntity);

        if (WhetherManager.instance.CheckWhether(new List<Whether.WhetherType> { whetherType })) WhetherManager.instance.StopWhether(whetherType);
        else WhetherManager.instance.StartWhether(whetherType);

        EndEvent();
    }
}
