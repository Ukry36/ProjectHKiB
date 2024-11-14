using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Weather
{
    public WeatherType type;
    public ParticleSystem visual;
    public string AMB;

    public enum WeatherType
    {
        Rain, ThunderStorm, Snow, SnowStorm
    }
}

public class WeatherManager : MonoBehaviour
{
    #region Singleton

    public static WeatherManager instance;

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

    [SerializeField] private List<Weather> allWeathers;
    private List<Weather.WeatherType> currentWeather = new();
    private Dictionary<Weather.WeatherType, Weather> WeatherDictionary = new();
    private bool isFadingIn;

    private void Start()
    {
        foreach (Weather weather in allWeathers)
        {
            WeatherDictionary.Add(weather.type, weather);
        }
    }

    private Weather GetWeather(Weather.WeatherType _type)
    {
        Weather weather = WeatherDictionary[_type];

        if (weather == null) { Debug.LogError("ERROR: Weather " + _type + " is missing!"); }

        return weather;
    }

    public bool CheckWeather(List<Weather.WeatherType> _weatherTypes)
    {
        for (int i = 0; i < _weatherTypes.Count; i++)
        {
            if (!currentWeather.Exists(weatherType => weatherType == _weatherTypes[i]))
                return false;
        }
        return true;
    }

    public void ChangeAreaWeathers(List<Weather.WeatherType> _weatherInfos, bool _immediate = false)
    {
        if (_weatherInfos == null || _weatherInfos.Count < 1)
        {
            StopAllWeathers(_immediate);
            return;
        }

        for (int i = 0; i < currentWeather.Count; i++)
        {
            if (!_weatherInfos.Exists(name => name == currentWeather[i]))
            {
                StopWeather(currentWeather[i], _immediate);
                currentWeather.Remove(currentWeather[i]);
            }
        }

        for (int i = 0; i < _weatherInfos.Count; i++)
        {
            if (!currentWeather.Exists(name => name == _weatherInfos[i]))
            {
                StartWeather(_weatherInfos[i], _immediate);
                currentWeather.Add(_weatherInfos[i]);
            }
        }
    }

    private void StopAllWeathers(bool _immediate)
    {
        if (currentWeather.Count > 0)
            for (int i = 0; i < currentWeather.Count; i++)
            {
                StopWeather(currentWeather[i], _immediate);
            }
        currentWeather = new();
    }

    public void StartWeather(Weather.WeatherType _weatherType, bool _immediate = false)
    {
        Weather weatherInfo = GetWeather(_weatherType);
        currentWeather.Add(_weatherType);
        AudioManager.instance.PlaySoundFlat(weatherInfo.AMB, 0, true, SoundType.AMB, 1);
        weatherInfo.visual.gameObject.SetActive(true);

        if (_immediate)
        {
            weatherInfo.visual.Play();
        }
        switch (weatherInfo.type)
        {
            case Weather.WeatherType.Rain:
                StartCoroutine(StartRain(weatherInfo));
                break;
            case Weather.WeatherType.ThunderStorm:
                StartCoroutine(StartRain(weatherInfo));
                break;
            case Weather.WeatherType.Snow:
                StartCoroutine(StartSnow(weatherInfo));
                break;
            case Weather.WeatherType.SnowStorm:
                StartCoroutine(StartSnowStorm(weatherInfo));
                break;
        }
    }

    public void StopWeather(Weather.WeatherType _weatherType, bool _immediate = false)
    {
        Weather weatherInfo = GetWeather(_weatherType);
        currentWeather.Remove(_weatherType);
        AudioManager.instance.StopLoopSound(weatherInfo.AMB, 1);
        if (_immediate)
        {
            weatherInfo.visual.Stop();
            weatherInfo.visual.gameObject.SetActive(false);
        }
        else
        {
            switch (weatherInfo.type)
            {
                case Weather.WeatherType.Rain:
                    StartCoroutine(StopRain(weatherInfo));
                    break;
                case Weather.WeatherType.ThunderStorm:
                    StartCoroutine(StopRain(weatherInfo));
                    break;
                case Weather.WeatherType.Snow:
                    StartCoroutine(StopSnow(weatherInfo));
                    break;
                case Weather.WeatherType.SnowStorm:
                    StartCoroutine(StopSnowStorm(weatherInfo));
                    break;
            }
        }
    }

    private void DropSome(Weather _weatherinfo, int _number = 10)
    {
        _weatherinfo.visual.Emit(_number);
    }

    private IEnumerator StartRain(Weather _weatherinfo)
    {
        isFadingIn = true;

        for (int i = 0; i < 10; i++)
        {
            DropSome(_weatherinfo, i);
            yield return new WaitForSeconds(0.05f);
        }
        _weatherinfo.visual.Play();

        isFadingIn = false;
    }
    private IEnumerator StopRain(Weather _weatherinfo)
    {
        yield return new WaitUntil(() => !isFadingIn);
        _weatherinfo.visual.Stop();
        for (int i = 10; i > 0; i--)
        {
            DropSome(_weatherinfo, i);
            yield return new WaitForSeconds(0.05f);
        }
    }


    private IEnumerator StartSnow(Weather _weatherinfo)
    {
        isFadingIn = true;

        for (int i = 0; i < 5; i++)
        {
            DropSome(_weatherinfo, i);
            yield return new WaitForSeconds(0.5f);
        }
        _weatherinfo.visual.Play();

        isFadingIn = false;
    }
    private IEnumerator StopSnow(Weather _weatherinfo)
    {
        yield return new WaitUntil(() => !isFadingIn);
        _weatherinfo.visual.Stop();
        yield return new WaitForSeconds(0.01f);
        for (int i = 5; i > 0; i--)
        {
            DropSome(_weatherinfo, i);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator StartSnowStorm(Weather _weatherinfo)
    {
        isFadingIn = true;

        for (int i = 0; i < 30; i++)
        {
            DropSome(_weatherinfo, i);
            yield return new WaitForSeconds(0.05f);
        }
        _weatherinfo.visual.Play();

        isFadingIn = false;
    }
    private IEnumerator StopSnowStorm(Weather _weatherinfo)
    {
        yield return new WaitUntil(() => !isFadingIn);
        _weatherinfo.visual.Stop();
        yield return new WaitForSeconds(0.01f);
        for (int i = 30; i > 0; i--)
        {
            DropSome(_weatherinfo, i);
            yield return new WaitForSeconds(0.05f);
        }
    }

}
