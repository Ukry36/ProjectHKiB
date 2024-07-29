using UnityEngine;

public class BGRenderer : MonoBehaviour
{
    #region Singleton
    static public BGRenderer instance;
    private void Awake()
    {
        if (instance == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    private SpriteRenderer spriteRenderer;

    private void Update()
    {
        this.transform.position = (Vector2)CameraManager.instance.transform.position;
    }

    public void RenderBackGround(Sprite _sprite)
    {
        spriteRenderer.sprite = _sprite;
    }
}
