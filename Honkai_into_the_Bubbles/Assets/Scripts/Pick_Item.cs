using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick_Item : MonoBehaviour
{
    public int ID;
    public int count = 1;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            if (InputManager.instance.ConfirmInput)
            {
                InventoryManager.instance.GetItem(ID, count);
                Destroy(this.gameObject);
            }
    }

}
