using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BGRenderer : MonoBehaviour
{
    #region Singleton
    static public BGRenderer instance;
    private void Awake()
    {
        if (instance == null)
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    private SpriteRenderer[] spriteRenderers;

    private void Update()
    {
        this.transform.position = (Vector2)CameraManager.instance.transform.position;
    }

    public void RenderBackGround(Sprite _sprite)
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = _sprite;
        }
    }
}
