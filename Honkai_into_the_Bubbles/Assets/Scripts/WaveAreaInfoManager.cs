using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WaveAreaInfoManager : MonoBehaviour
{
    public delegate void WaveTransition();
    public event WaveTransition OnWaveTransition;
    public AreaInfo beforeAreaInfo;
    public AreaInfo frontAreaInfo;
    public AreaInfo middleAreaInfo;
    public AreaInfo rearAreaInfo;
    public AreaInfo afterAreaInfo;
    public Light2D fadeLight;
    private Coroutine transitionCoroutine;
    private bool isFading = false;

    private enum waveTransitionType
    {
        FrontToMiddle,
        MiddleToRear
    }

    public void BeforeToFrontTransition()
    {
        Debug.Log("AreaInfo Before to Front");
        beforeAreaInfo.gameObject.SetActive(false);
        frontAreaInfo.gameObject.SetActive(true);
    }

    public void FrontToMiddleTransition(float _duration, float _intensity)
    {
        Debug.Log("AreaInfo Front to Middle");
        if (isFading) return;

        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(TransitionCoroutine(_duration, _intensity, waveTransitionType.FrontToMiddle));
    }

    public void MiddleToRearTransition(float _duration, float _intensity)
    {
        Debug.Log("AreaInfo Middle to Rear");
        if (isFading) return;

        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(TransitionCoroutine(_duration, _intensity, waveTransitionType.MiddleToRear));
    }

    public void RearToAfterTransition()
    {
        Debug.Log("AreaInfo Rear to After");
        rearAreaInfo.gameObject.SetActive(false);
        afterAreaInfo.gameObject.SetActive(true);
    }

    private IEnumerator TransitionCoroutine(float _duration, float _intensity, waveTransitionType _type)
    {
        isFading = true;
        fadeLight.gameObject.SetActive(true);

        // fadeOut
        fadeLight.intensity = 0;
        float t = 0f;
        MenuManager.instance.SetFadeColor(Color.white);
        MenuManager.instance.StartCoroutine(MenuManager.instance.FadeCoroutine(1, _duration));
        while (t < _duration)
        {
            t += Time.deltaTime;
            fadeLight.intensity = Mathf.Lerp(0, _intensity, t / _duration);
            yield return null;
        }

        // change BG and Light
        switch (_type)
        {
            case waveTransitionType.FrontToMiddle:
                frontAreaInfo.globalLight.gameObject.SetActive(false);
                frontAreaInfo.gameObject.SetActive(false);
                middleAreaInfo.globalLight.gameObject.SetActive(true);
                middleAreaInfo.gameObject.SetActive(true);
                break;
            case waveTransitionType.MiddleToRear:
                middleAreaInfo.globalLight.gameObject.SetActive(false);
                middleAreaInfo.gameObject.SetActive(false);
                rearAreaInfo.globalLight.gameObject.SetActive(true);
                rearAreaInfo.gameObject.SetActive(true);
                break;
        }

        // fadeIn
        fadeLight.intensity = _intensity;
        t = 0f;
        MenuManager.instance.StartCoroutine(MenuManager.instance.FadeCoroutine(0, _duration));
        while (t < _duration)
        {
            t += Time.deltaTime;
            fadeLight.intensity = Mathf.Lerp(_intensity, 0, t / _duration);
            yield return null;
        }

        isFading = false;
        fadeLight.gameObject.SetActive(false);
        OnWaveTransition?.Invoke();
    }
}
