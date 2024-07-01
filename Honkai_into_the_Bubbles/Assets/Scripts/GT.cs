using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GT : MonoBehaviour
{
    [SerializeField] private Vector2 coordinate;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private static GraffitiSystem GS;
    private int GTPCCI = 0;
    private void Awake()
    {
        coordinate = new Vector2(this.transform.position.x, this.transform.position.y);
    }

    private void OnEnable()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        GTPCCI = 0;
    }

    public static void SetGS(GraffitiSystem _GS)
    {
        GS = _GS;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GS.AddTile(coordinate);
            List<Color> colors = PlayerManager.instance.ThemeColors;
            spriteRenderer.color = colors[GTPCCI++ % colors.Count];
        }
    }
}