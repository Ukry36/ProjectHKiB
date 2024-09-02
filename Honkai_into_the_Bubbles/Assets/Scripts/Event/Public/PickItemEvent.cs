using System;
using System.Collections.Generic;

[Serializable]
public class PickItemInfo
{
    public int ID;
    public int count;
    public PickItemInfo(int _ID, int _count = 1)
    {
        ID = _ID;
        count = _count;
    }
}
public class PickItemEvent : Event
{
    public List<PickItemInfo> Items = new();

    protected override void StartEvent(Status _interactedEntity)
    {
        StartCoroutine(Cooltime());
        foreach (PickItemInfo item in Items)
            if (item.count > 0)
                InventoryManager.instance.GetItem(item.ID, item.count);
        EndEvent();

    }

}
