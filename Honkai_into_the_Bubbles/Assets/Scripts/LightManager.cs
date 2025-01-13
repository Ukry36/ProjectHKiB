using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Color = UnityEngine.Color;

public class LightManager : MonoBehaviour
{
    public delegate void WaveTransition();
    public event WaveTransition OnWaveTransition;
    public AreaInfo area;
    public Light2D fadeLight;
    public GameObject obj;
    // 현재 활성화된 조명을 관리
    private Light2D activeLight;
    private Coroutine transitionCoroutine;
    private bool isFading = false;

    private void Start()
    {
        activeLight = area.frontWaveLight;
    }

    public void SetLighting(Light2D newLight, float transitionDuration)
    {
        if (isFading) return;

        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }

        transitionCoroutine = StartCoroutine(TransitionLighting(newLight, transitionDuration));
    }

    private IEnumerator TransitionLighting(Light2D newLight, float duration)
    {
        // 현재 조명의 색상과 강도
        Color initialColor = activeLight != null ? activeLight.color : Color.black;
        float initialIntensity = activeLight != null ? activeLight.intensity : 0f;

        // 새로운 조명의 색상과 강도
        Color targetColor = newLight.color;
        float targetIntensity = newLight.intensity;

        // 조명 전환
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // 색상과 강도를 점진적으로 변경
            Color blendedColor = Color.Lerp(initialColor, targetColor, t);
            float blendedIntensity = Mathf.Lerp(initialIntensity, targetIntensity, t);

            if (activeLight != null)
            {
                activeLight.color = blendedColor;
                activeLight.intensity = blendedIntensity;
            }
            yield return null;
        }
        OnWaveTransition?.Invoke();
    }
    public void ActivateWaveLight(AreaInfo areaInfo, string waveType, float transitionDuration)
    {
        Light2D targetLight = null;
        switch (waveType)
        {
            case "frontWave":
                Debug.Log("front");
                targetLight = areaInfo.frontWaveLight;
                break;
            case "middleWave":
                Debug.Log("middle");
                transitionDuration = transitionDuration / 2f;
                targetLight = areaInfo.middleWaveLight;
                transitionCoroutine = StartCoroutine(FadingAndBackground(targetLight, transitionDuration));
                break;
            case "backWave":
                Debug.Log("back");
                targetLight = areaInfo.backWaveLight;
                break;
        }

        if (targetLight != null)
        {
            SetLighting(targetLight, transitionDuration);
        }
    }

    private IEnumerator FadingAndBackground(Light2D targetLight, float transitionDuration)
    {
        Debug.Log("fade");
        isFading = true;
        float initialIntensity = activeLight != null ? activeLight.intensity : 0f;

        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            float blendedIntensity = Mathf.Lerp(initialIntensity, 20, t);

            if (activeLight != null)
            {
                activeLight.intensity = blendedIntensity;
            }
            yield return null;
        }

        obj = GameObject.Find("CinemachineBrain");
        if (obj != null)
        {
            CameraManager cameraManager = obj.GetComponent<CameraManager>();
            if(cameraManager != null)
            {
                cameraManager.SetAfterBG(area);
            }
        }

        isFading = false;

        if (targetLight != null)
        {
            SetLighting(targetLight, transitionDuration);
        }
    }
}
