using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GT : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private int GTPCCI = 0;

    private void OnEnable()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        GTPCCI = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GraffitiSystemManager.instance.playerGS.AddTile(this.transform.localPosition);
            List<Color> colors = PlayerManager.instance.ThemeColors;
            spriteRenderer.color = colors[GTPCCI++ % colors.Count];
        }
    }
}