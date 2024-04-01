using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick_Item : MonoBehaviour
{
    public int ID;
    public int count = 1;
    public GameObject ItemObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DetectCoroutine(collision));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines();
        }
    }

    IEnumerator DetectCoroutine(Collider2D collision)
    {
        yield return new WaitUntil(()=> InputManager.instance.ConfirmInput );
        InventoryManager.instance.GetItem(ID, count);
        Destroy(this.gameObject);
        if(ItemObject != null)
            Destroy(ItemObject);
    }
}
