using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whetherTest : Event
{
    public Weather.WeatherType whetherType;
    protected override void StartEvent(Status _interactedEntity)
    {
        StartCoroutine(Cooltime());
        if (WeatherManager.instance.CheckWeather(new List<Weather.WeatherType> { whetherType })) WeatherManager.instance.StopWeather(whetherType);
        else WeatherManager.instance.StartWeather(whetherType);

        EndEvent();
    }
}
