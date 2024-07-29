using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{

    public int ID;
    public string Name;
    public string Description;
    public int Count;
    public Sprite Icon;
    public ItemType Type;
    public int Equipped; // 0: not eqd, 1: eqd, 2: transited, but not prime 3: prime or transited and prime
    public int[] TransitionPath;

    public enum ItemType
    {
        Effect,
        Use,
        Cloth,
        ETC
    }

    public Item(int _ID = 0, string _Name = "", string _Desc = "", ItemType _Type = ItemType.Effect, int _Count = 1, int _Equipped = 0, int[] _Transition = null)
    {
        ID = _ID;
        Name = _Name;
        Description = _Desc;
        Count = _Count;
        Type = _Type;
        Equipped = _Equipped;
        TransitionPath = _Transition;
        Icon = Resources.Load("itemIcon/" + _ID.ToString(), typeof(Sprite)) as Sprite;
    }
}
