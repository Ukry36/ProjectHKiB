using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhetherManager : MonoBehaviour
{
    #region Singleton

    public static WhetherManager instance;

    private void Awake()
    {
        if(instance == null)
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

    private AudioManager theAudio;
    public ParticleSystem rain;
    public ParticleSystem snow;
    public string rain_sound;

    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    public void StartRain()
    {
        
        StopAllCoroutines();
        StartCoroutine(StartRainCoroutine());
    }

    public void StopRain()
    {
        
        StopAllCoroutines();
        StartCoroutine(StopRainCoroutine());
    }

    public void RainDrop(int _number = 10)
    {
        rain.Emit(_number);
    }

    IEnumerator StartRainCoroutine()
    {
        theAudio.Play(rain_sound);
        for(int i=40; i>0;i--)
        {
            RainDrop(40-i);
            theAudio.SetVolume(rain_sound,0.025f*(40-i));
            yield return new WaitForSeconds(0.05f);
        }
        snow.Play();
    }
    IEnumerator StopRainCoroutine()
    {
        rain.Stop();
        yield return new WaitForSeconds(0.01f);
        for(int j=0; j<40;j++)
        {
            RainDrop(40-j);
            theAudio.SetVolume(rain_sound,0.025f*(40-j));
            yield return new WaitForSeconds(0.05f);
        }
        theAudio.Stop(rain_sound);
    }


    public void StartSnow()
    {
        StopAllCoroutines();
        StartCoroutine(StartSnowCoroutine());
    }

    public void StopSnow()
    {
        StopAllCoroutines();
        StartCoroutine(StopSnowCoroutine());
    }

    public void SnowFall(int _number = 10)
    {
        snow.Emit(_number);
    }

    IEnumerator StartSnowCoroutine()
    {
        for(int i=40; i>0;i--)
        {
            SnowFall(40-i);
            yield return new WaitForSeconds(0.05f);
        }
        snow.Play();
    }
    IEnumerator StopSnowCoroutine()
    {
        snow.Stop();
        yield return new WaitForSeconds(0.01f);
        for(int j=0; j<40;j++)
        {
            SnowFall(40-j);
            yield return new WaitForSeconds(0.05f);
        }
    }

}
