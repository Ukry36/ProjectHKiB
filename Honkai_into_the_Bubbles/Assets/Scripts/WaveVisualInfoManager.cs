using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public class VisualInfo
{
    public AreaInfo areaInfo;
    public Light2D light;
}

public class WaveVisualInfoManager : MonoBehaviour
{
    public delegate void WaveTransition();
    public event WaveTransition OnWaveTransition;
    public VisualInfo beforeInfo;
    public VisualInfo frontWaveInfo;
    public VisualInfo middleWaveInfo;
    public VisualInfo rearWaveInfo;
    public VisualInfo afterInfo;
    public Light2D fadeLight;
    private Coroutine transitionCoroutine;
    private bool isFading = false;

    public GameObject AfterGrid;
    public GameObject BeforeGrid;

    [SerializeField] private GameObject QuaDecoObjects;

    private enum waveTransitionType
    {
        FrontToMiddle,
        MiddleToRear
    }

    private void Start()
    {
        QuaDecoObjects.SetActive(false);
        fadeLight.enabled = false;

        AfterGrid.SetActive(false);
        BeforeGrid.SetActive(true);
    }

    public void BeforeToFrontTransition()
    {
        if (beforeInfo.areaInfo.gameObject && frontWaveInfo.areaInfo.gameObject)
        {
            beforeInfo.areaInfo.gameObject.SetActive(false);
            frontWaveInfo.areaInfo.gameObject.SetActive(true);
        }
    }

    public void FrontToMiddleTransition(float _duration, float _intensity)
    {
        if (isFading) return;

        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(TransitionCoroutine(_duration, _intensity, waveTransitionType.FrontToMiddle));
    }

    public void MiddleToRearTransition(float _duration, float _intensity)
    {
        if (isFading) return;

        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(TransitionCoroutine(_duration, _intensity, waveTransitionType.MiddleToRear));
    }

    public void RearToAfterTransition()
    {
        rearWaveInfo.areaInfo.gameObject.SetActive(false);
        afterInfo.areaInfo.gameObject.SetActive(true);
    }

    private IEnumerator TransitionCoroutine(float _duration, float _intensity, waveTransitionType _type)
    {
        isFading = true;
        fadeLight.enabled = true;

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
                frontWaveInfo.light.enabled = false;
                frontWaveInfo.areaInfo.gameObject.SetActive(false);
                middleWaveInfo.light.enabled = true;
                middleWaveInfo.areaInfo.gameObject.SetActive(true);
                QuaDecoObjects.SetActive(true);
                BeforeGrid.SetActive(false);
                break;
            case waveTransitionType.MiddleToRear:
                middleWaveInfo.light.enabled = false;
                middleWaveInfo.areaInfo.gameObject.SetActive(false);
                rearWaveInfo.light.enabled = true;
                rearWaveInfo.areaInfo.gameObject.SetActive(true);
                QuaDecoObjects.SetActive(false);
                AfterGrid.SetActive(true);
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
        fadeLight.enabled = false;
        OnWaveTransition?.Invoke();
    }
}
