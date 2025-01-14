using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    #region Singleton
    static public PostProcessManager instance;
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

    private Volume volume;

    private ChromaticAberration chromaticAberration;

    private Coroutine coroutine;

    private void Start()
    {
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        volume = FindObjectOfType<Volume>();
    }

    public void ChromaticImpact(float _inTime, float _outTime)
    {
        if (coroutine != null) StopCoroutine(coroutine);
        volume.profile.TryGet(out chromaticAberration);
        chromaticAberration.active = true;
        chromaticAberration.intensity.value = 0;
        coroutine = StartCoroutine(ChromaticImpactCoroutine(_inTime, _outTime));
    }

    private IEnumerator ChromaticImpactCoroutine(float _inTime, float _outTime)
    {
        for (int i = 0; i < _inTime / Time.deltaTime; i++)
        {
            chromaticAberration.intensity.value += 1 / _inTime * Time.deltaTime;
            yield return null;
        }
        chromaticAberration.intensity.value = 1f;

        for (int i = 0; i < _outTime / Time.deltaTime; i++)
        {
            chromaticAberration.intensity.value -= 1 / _outTime * Time.deltaTime;
            yield return null;
        }
        chromaticAberration.intensity.value = 0f;
    }
}
