using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GT : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer nSpriteRenderer;
    private int GTPCCI = 0;

    private void OnEnable()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        nSpriteRenderer.color = new Color(1, 1, 1, 0);
        GTPCCI = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            List<Color> colors = PlayerManager.instance.ThemeColors;
            spriteRenderer.color = colors[(GraffitiSystemManager.instance.totalGraffitiCount + GTPCCI) % colors.Count];

            GraffitiSystemManager.instance.playerGS.AddTile(this.transform.localPosition);
            GraffitiSystemManager.instance.GraffitiPath(this.transform.position, GTPCCI++);
        }

        if ((LayerManager.instance.graffitiWallLayer & (1 << other.gameObject.layer)) != 0)
        {
            nSpriteRenderer.color = new Color(1, 0, 0, 0.5f);
        }
    }
}