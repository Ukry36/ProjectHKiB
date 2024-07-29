using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Count;
    [SerializeField] private Image Slot;

    private Color RED = new(1f, 0f, 0f, 1f),
        YELLOW = new(1f, 0.6f, 0, 1f),
        PURPLE = new(0.5f, 0, 1f, 1f),
        BLUE = new(0, 0.55f, 1f, 1f),
        GREEN = new(0.5f, 0.9f, 0, 1f),
        BLACK = new(0.3f, 0.3f, 0.3f, 1f),
        NONE = new(0, 0, 0, 0),
        PINK = new(1f, 0.32f, 0.48f, 1f),
        DARKBLUE = new(0.32f, 0.48f, 1f, 1f),
        MINT = new(0.65f, 1f, 0.8f, 1f),
        WHITE = new(1f, 1f, 1f, 1f);


    public void AddItem(Item _item)
    {
        Name.text = _item.Name;
        Icon.sprite = _item.Icon;

        if (Item.ItemType.Use == _item.Type)
        {
            if (_item.Count > 1)
                Count.text = "x " + _item.Count.ToString();
            else
                Count.text = "";
        }

        switch (_item.Equipped)
        {
            case 3:
                Slot.color = PINK; break;
            case 2:
                Slot.color = DARKBLUE; break;
            case 1:
                Slot.color = MINT; break;
            default:
                Slot.color = WHITE; break;
        }
    }

    public void RemoveItem()
    {
        Name.text = "";
        Count.text = "";
        Icon.sprite = null;
    }
}
