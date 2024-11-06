using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaInfo : MonoBehaviour
{
    public PolygonCollider2D cameraBound;
    public Sprite backGround;
    public List<string> areaBGMs;
    public float fadeTime;
    public List<Weather.WeatherType> areaWhetherTypes;
}
