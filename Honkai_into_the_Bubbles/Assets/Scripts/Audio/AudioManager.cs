using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    BGM,
    SFX,
    AMB
}

public class AudioManager : MonoBehaviour
{
    #region Singleton

    public static AudioManager instance;

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
    #endregion Singleton

    [SerializeField] private AudioMixer audioMixer;
    private float currentBGMVolume, currentSFXVolume;
    private Dictionary<string, AudioClip> ClipsDictionary = new();
    [SerializeField] private AudioClip[] PreloadedClips;
    private List<SoundPlayer> SoundPlayers = new();
    private List<string> CurrentAreaBGMs = new();

    [SerializeField] private GameObject SoundPlayerPrefab;
    [SerializeField] private GameObject FlatSoundPlayerPrefab;

    public string hpSFX;
    public string gpSFX;
    public string gpDrainSFX;


    private void Start()
    {
        foreach (AudioClip clip in PreloadedClips)
        {
            ClipsDictionary.Add(clip.name, clip);
        }
    }

    private AudioClip GetClip(string _clipName)
    {
        AudioClip clip = ClipsDictionary[_clipName];

        if (clip == null) { Debug.LogError("ERROR: AudioClip " + _clipName + " is missing!"); }

        return clip;
    }

    // 사운드를 재생할 때, 루프 형태로 재생된 경우 나중에 제거하기위해 리스트에 저장
    private void AddToList(SoundPlayer _soundPlayer)
    {
        SoundPlayers.Add(_soundPlayer);
    }

    public void ChangeAreaBGMs(List<string> _clipNames, float _fadeTime)
    {
        if (_clipNames == null || _clipNames.Count < 1)
        {
            StopAllAreaBGMs(_fadeTime);
            return;
        }

        for (int i = 0; i < CurrentAreaBGMs.Count; i++)
        {
            if (!_clipNames.Exists(name => name == CurrentAreaBGMs[i]))
            {
                StopLoopSound(CurrentAreaBGMs[i], _fadeTime);
                CurrentAreaBGMs.Remove(CurrentAreaBGMs[i]);
            }
        }

        for (int i = 0; i < _clipNames.Count; i++)
        {
            if (!CurrentAreaBGMs.Exists(name => name == _clipNames[i]))
            {
                PlaySoundFlat(_clipNames[i], 0, true, SoundType.BGM, _fadeTime);
                CurrentAreaBGMs.Add(_clipNames[i]);
            }
        }
    }

    private void StopAllAreaBGMs(float _fadeOutTime = 1)
    {
        if (CurrentAreaBGMs.Count > 0)
            for (int i = 0; i < CurrentAreaBGMs.Count; i++)
            {
                StopLoopSound(CurrentAreaBGMs[i], _fadeOutTime);
            }
        CurrentAreaBGMs = new();
    }

    // 루프형태 사운드 정지, 제거
    public void StopLoopSound(string _clipName, float _fadeOutTime = 1)
    {
        if (_clipName == "") return;

        foreach (SoundPlayer audioPlayer in SoundPlayers)
        {
            if (audioPlayer.ClipName == _clipName && !audioPlayer.isFadingOut)
            {
                if (_fadeOutTime <= 0)
                {
                    SoundPlayers.Remove(audioPlayer);
                    audioPlayer.gameObject.SetActive(false);
                }
                else
                {
                    StartCoroutine(FadeOut(audioPlayer, _fadeOutTime));
                }
                return;
            }
        }

        Debug.LogWarning(_clipName + "ERROR: AudioClip " + _clipName + " is missing!");
    }

    private IEnumerator FadeOut(SoundPlayer _audioPlayer, float _fadeOutTime)
    {
        yield return _audioPlayer.FadeOutLoop(_fadeOutTime);
        SoundPlayers.Remove(_audioPlayer);
        _audioPlayer.gameObject.SetActive(false);
    }

    public void PlaySoundFlat(string _clipName, float _delay = 0f, bool _isLoop = false, SoundType _type = SoundType.SFX, float _fadeIn = 0, float _fadeOut = 0)
    {
        if (_clipName == "" || _clipName == null) return;
        GameObject obj = PoolManager.instance.ReuseGameObject(FlatSoundPlayerPrefab, this.transform.position, quaternion.identity);
        obj.transform.parent = this.transform;

        if (obj.TryGetComponent(out SoundPlayer soundPlayer))
        {
            //루프를 사용하는경우 사운드를 저장한다.
            if (_isLoop) AddToList(soundPlayer);

            soundPlayer.InitSound(GetClip(_clipName));
            soundPlayer.Play(audioMixer.FindMatchingGroups(_type.ToString())[0], _delay, _isLoop, _fadeIn, _fadeOut);
        }
    }

    public void PlaySoundFlat(string[] _clipNames, float _delay = 0f, bool _isLoop = false, SoundType _type = SoundType.SFX, float _fadeIn = 0, float _fadeOut = 0)
    => PlaySoundFlat(_clipNames.Length > 0 ? _clipNames[UnityEngine.Random.Range(0, _clipNames.Length)] : "", _delay, _isLoop, _type, _fadeIn, _fadeOut);

    public void PlaySound(string _clipName, Transform _audioTarget, float _delay = 0f, bool _isLoop = false, SoundType _type = SoundType.SFX, bool _attachToTarget = false, float _minDistance = 0.0f, float _maxDistance = 50.0f, float _fadeIn = 0, float _fadeOut = 0)
    {
        if (_clipName == "" || _clipName == null) return;
        GameObject obj = PoolManager.instance.ReuseGameObject(SoundPlayerPrefab, _audioTarget.transform.position, quaternion.identity);
        if (_attachToTarget) obj.transform.parent = _audioTarget;

        if (obj.TryGetComponent(out SoundPlayer soundPlayer))
        {
            //루프를 사용하는경우 사운드를 저장한다.
            if (_isLoop) AddToList(soundPlayer);

            soundPlayer.InitSound(GetClip(_clipName), _minDistance, _maxDistance);
            soundPlayer.Play(audioMixer.FindMatchingGroups(_type.ToString())[0], _delay, _isLoop, _fadeIn, _fadeOut);
        }
    }

    public void PlaySound(string[] _clipNames, Transform _audioTarget, float _delay = 0f, bool _isLoop = false, SoundType _type = SoundType.SFX, bool _attachToTarget = false, float _minDistance = 0.0f, float _maxDistance = 50.0f, float _fadeIn = 0, float _fadeOut = 0)
    => PlaySound(_clipNames.Length > 0 ? _clipNames[UnityEngine.Random.Range(0, _clipNames.Length)] : "", _audioTarget, _delay, _isLoop, _type, _attachToTarget, _minDistance, _maxDistance, _fadeIn, _fadeOut);

    // used in option init
    public void InitVolumes(float _bgm, float _sfx, float _amb)
    {
        SetVolume(SoundType.BGM, _bgm);
        SetVolume(SoundType.SFX, _sfx);
        SetVolume(SoundType.AMB, _amb);
    }

    public void SetVolume(SoundType _type, float _value)
    {
        audioMixer.SetFloat(_type.ToString(), _value);
    }

}