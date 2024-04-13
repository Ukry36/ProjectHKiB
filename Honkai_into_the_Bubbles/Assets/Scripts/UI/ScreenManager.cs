using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    #region Singleton
    public static ScreenManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    [SerializeField] private Image fadeImage;

    private Color tempColor;
    public void SetFadeColor(Color _color)
    {
        _color.a = fadeImage.color.a;
        tempColor = _color;
    }

    public IEnumerator FadeCoroutine(float _opacity, float _fadeTime)
    {
        StopCoroutine(nameof(FadeOutCoroutine));
        if (tempColor.a < _opacity)
            yield return FadeOutCoroutine(_opacity, 1f / _fadeTime);
        else
            yield return FadeInCoroutine(_opacity, 1f / _fadeTime);
    }
    private IEnumerator FadeOutCoroutine(float _opacity, float _speed)
    {
        while (tempColor.a < _opacity)
        {
            tempColor.a += _speed * Time.deltaTime;
            fadeImage.color = tempColor;
            yield return null;
        }
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, _opacity);
    }
    private IEnumerator FadeInCoroutine(float _opacity, float _speed)
    {
        while (tempColor.a > _opacity)
        {
            tempColor.a -= _speed * Time.deltaTime;
            fadeImage.color = tempColor;
            yield return null;
        }
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, _opacity);
    }



}
