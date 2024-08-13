using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Whether
{
    public WhetherType type;
    public ParticleSystem visual;
    public string AMB;

    public enum WhetherType
    {
        Rain, ThunderStorm, Snow, SnowStorm
    }
}

public class WhetherManager : MonoBehaviour
{
    #region Singleton

    public static WhetherManager instance;

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

    [SerializeField] private List<Whether> allWhethers;
    private List<Whether.WhetherType> currentWhether = new();
    private Dictionary<Whether.WhetherType, Whether> WhetherDictionary = new();
    private bool isFadingIn;

    private void Start()
    {
        foreach (Whether whether in allWhethers)
        {
            WhetherDictionary.Add(whether.type, whether);
        }
    }

    private Whether GetWhether(Whether.WhetherType _type)
    {
        Whether whether = WhetherDictionary[_type];

        if (whether == null) { Debug.LogError("ERROR: Whether " + _type + " is missing!"); }

        return whether;
    }

    public bool CheckWhether(List<Whether.WhetherType> _whetherTypes)
    {
        for (int i = 0; i < _whetherTypes.Count; i++)
        {
            if (!currentWhether.Exists(whetherType => whetherType == _whetherTypes[i]))
                return false;
        }
        return true;
    }

    public void ChangeAreaWhethers(List<Whether.WhetherType> _whetherInfos, bool _immediate = false)
    {
        if (_whetherInfos == null || _whetherInfos.Count < 1)
        {
            StopAllWhethers(_immediate);
            return;
        }

        for (int i = 0; i < currentWhether.Count; i++)
        {
            if (!_whetherInfos.Exists(name => name == currentWhether[i]))
            {
                StopWhether(currentWhether[i], _immediate);
                currentWhether.Remove(currentWhether[i]);
            }
        }

        for (int i = 0; i < _whetherInfos.Count; i++)
        {
            if (!currentWhether.Exists(name => name == _whetherInfos[i]))
            {
                StartWhether(_whetherInfos[i], _immediate);
                currentWhether.Add(_whetherInfos[i]);
            }
        }
    }

    private void StopAllWhethers(bool _immediate)
    {
        if (currentWhether.Count > 0)
            for (int i = 0; i < currentWhether.Count; i++)
            {
                StopWhether(currentWhether[i], _immediate);
            }
        currentWhether = new();
    }

    public void StartWhether(Whether.WhetherType _whetherType, bool _immediate = false)
    {
        Whether whetherInfo = GetWhether(_whetherType);
        currentWhether.Add(_whetherType);
        AudioManager.instance.PlaySoundFlat(whetherInfo.AMB, 0, true, SoundType.AMB, 1);
        whetherInfo.visual.gameObject.SetActive(true);

        if (_immediate)
        {
            whetherInfo.visual.Play();
        }
        switch (whetherInfo.type)
        {
            case Whether.WhetherType.Rain:
                StartCoroutine(StartRain(whetherInfo));
                break;
            case Whether.WhetherType.ThunderStorm:
                StartCoroutine(StartRain(whetherInfo));
                break;
            case Whether.WhetherType.Snow:
                StartCoroutine(StartSnow(whetherInfo));
                break;
            case Whether.WhetherType.SnowStorm:
                StartCoroutine(StartSnowStorm(whetherInfo));
                break;
        }
    }

    public void StopWhether(Whether.WhetherType _whetherType, bool _immediate = false)
    {
        Whether whetherInfo = GetWhether(_whetherType);
        currentWhether.Remove(_whetherType);
        AudioManager.instance.StopLoopSound(whetherInfo.AMB, 1);
        if (_immediate)
        {
            whetherInfo.visual.Stop();
            whetherInfo.visual.gameObject.SetActive(false);
        }
        else
        {
            switch (whetherInfo.type)
            {
                case Whether.WhetherType.Rain:
                    StartCoroutine(StopRain(whetherInfo));
                    break;
                case Whether.WhetherType.ThunderStorm:
                    StartCoroutine(StopRain(whetherInfo));
                    break;
                case Whether.WhetherType.Snow:
                    StartCoroutine(StopSnow(whetherInfo));
                    break;
                case Whether.WhetherType.SnowStorm:
                    StartCoroutine(StopSnowStorm(whetherInfo));
                    break;
            }
        }
    }

    private void DropSome(Whether _whetherinfo, int _number = 10)
    {
        _whetherinfo.visual.Emit(_number);
    }

    private IEnumerator StartRain(Whether _whetherinfo)
    {
        isFadingIn = true;

        for (int i = 0; i < 10; i++)
        {
            DropSome(_whetherinfo, i);
            yield return new WaitForSeconds(0.05f);
        }
        _whetherinfo.visual.Play();

        isFadingIn = false;
    }
    private IEnumerator StopRain(Whether _whetherinfo)
    {
        yield return new WaitUntil(() => !isFadingIn);
        _whetherinfo.visual.Stop();
        for (int i = 10; i > 0; i--)
        {
            DropSome(_whetherinfo, i);
            yield return new WaitForSeconds(0.05f);
        }
    }


    private IEnumerator StartSnow(Whether _whetherinfo)
    {
        isFadingIn = true;

        for (int i = 0; i < 5; i++)
        {
            DropSome(_whetherinfo, i);
            yield return new WaitForSeconds(0.5f);
        }
        _whetherinfo.visual.Play();

        isFadingIn = false;
    }
    private IEnumerator StopSnow(Whether _whetherinfo)
    {
        yield return new WaitUntil(() => !isFadingIn);
        _whetherinfo.visual.Stop();
        yield return new WaitForSeconds(0.01f);
        for (int i = 5; i > 0; i--)
        {
            DropSome(_whetherinfo, i);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator StartSnowStorm(Whether _whetherinfo)
    {
        isFadingIn = true;

        for (int i = 0; i < 30; i++)
        {
            DropSome(_whetherinfo, i);
            yield return new WaitForSeconds(0.05f);
        }
        _whetherinfo.visual.Play();

        isFadingIn = false;
    }
    private IEnumerator StopSnowStorm(Whether _whetherinfo)
    {
        yield return new WaitUntil(() => !isFadingIn);
        _whetherinfo.visual.Stop();
        yield return new WaitForSeconds(0.01f);
        for (int i = 30; i > 0; i--)
        {
            DropSome(_whetherinfo, i);
            yield return new WaitForSeconds(0.05f);
        }
    }

}
