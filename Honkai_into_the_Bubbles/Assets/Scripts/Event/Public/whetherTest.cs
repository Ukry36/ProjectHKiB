using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whetherTest : Event
{
    public Whether.WhetherType whetherType;
    protected override void StartEvent(Status _interactedEntity)
    {
        if (!isCooltime)
        {
            if (WhetherManager.instance.CheckWhether(new List<Whether.WhetherType> { whetherType })) WhetherManager.instance.StopWhether(whetherType);
            else WhetherManager.instance.StartWhether(whetherType);

            EndEvent();
        }
    }
}
