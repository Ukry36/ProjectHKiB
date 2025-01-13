using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AreaInfo : MonoBehaviour
{
    public PolygonCollider2D cameraBound;
    public float cameraResolusion;
    public float changeTime;
    public CinemachineBlendDefinition.Style changeStyle;
    public Sprite backGround;
    public Sprite afterBackGround;
    public List<string> areaBGMs;
    public float fadeTime;
    public List<Weather.WeatherType> areaWhetherTypes;

    public Light2D frontWaveLight;
    public Light2D middleWaveLight;
    public Light2D backWaveLight;
}
