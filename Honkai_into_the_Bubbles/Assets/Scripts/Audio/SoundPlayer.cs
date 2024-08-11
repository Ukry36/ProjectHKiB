using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    [HideInInspector] public bool isFadingOut;
    [HideInInspector] public bool isFadingIn;
    public string ClipName
    {
        get { return audioSource.clip.name; }
    }

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioMixerGroup _audioMixer, float _delay, bool _isLoop, float _fadeInTime, float _fadeOutTime)
    {
        StopAllCoroutines();
        audioSource.outputAudioMixerGroup = _audioMixer;
        audioSource.loop = _isLoop;
        if (_fadeInTime > 0) StartCoroutine(FadeIn(_fadeInTime));
        audioSource.Play();

        if (!_isLoop) StartCoroutine(PlayNonLoopAudio(audioSource.clip.length, _fadeOutTime));
    }

    public void InitSound(AudioClip _clip)
    {
        audioSource.clip = _clip;
    }

    public void InitSound(AudioClip _clip, float _minDistance, float _maxDistance)
    {
        isFadingOut = false;
        audioSource.clip = _clip;
        audioSource.spatialBlend = 1.0f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = _minDistance;
        audioSource.maxDistance = _maxDistance;
    }

    private IEnumerator PlayNonLoopAudio(float _clipLength, float _fadeOutTime)
    {
        while (_clipLength > 0)
        {
            yield return null;
            _clipLength -= Time.deltaTime;
            if (_fadeOutTime > 0 && _clipLength < _fadeOutTime)
            {
                StartCoroutine(FadeOutLoop(_fadeOutTime));
            }
        }
        this.gameObject.SetActive(false);
    }

    private IEnumerator FadeIn(float _fadeInTime)
    {
        isFadingIn = true;

        StopCoroutine(nameof(FadeOutLoop));
        audioSource.volume = 0;
        float timer = _fadeInTime;
        float t = Time.deltaTime;
        while (audioSource.volume < 1 || timer > 0)
        {
            audioSource.volume += t / _fadeInTime;
            if (audioSource.volume > 1) audioSource.volume = 1;
            timer -= Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 1;

        isFadingIn = false;
    }

    public IEnumerator FadeOutLoop(float _fadeOutTime)
    {
        isFadingOut = true;

        yield return new WaitUntil(() => !isFadingIn);
        float timer = _fadeOutTime;
        while (timer > 0)
        {
            audioSource.volume -= Time.deltaTime / _fadeOutTime;
            timer -= Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 0;

        isFadingOut = false;
    }
}