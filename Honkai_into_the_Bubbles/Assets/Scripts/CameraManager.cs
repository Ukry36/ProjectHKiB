using System;
using System.Collections;
using System.Diagnostics;
using Cinemachine;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    #region Singleton
    static public CameraManager instance;
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
    #endregion
    
    [SerializeField] private CinemachineVirtualCamera[] Cameras = new CinemachineVirtualCamera[2];
    [SerializeField] private CinemachineBrain CBrain;

    private float OriginalRes = 5;
    private int CurrentCamera = 0; // 0 or 1
    public bool freeze = false;

    private void Start()
    {
        Cameras[CurrentCamera].Priority = 11;
    }


// 0 to 1, 1 to 0
    private int FlipNum(int _i)
    { int i = (_i + 1) % 2; return i; }

// OWO
    public void Zoom(float _res, float _blendTime, 
    CinemachineBlendDefinition.Style _style = CinemachineBlendDefinition.Style.EaseOut)
    {
        CurrentCamera = FlipNum(CurrentCamera); 
        CBrain.m_DefaultBlend.m_Time = _blendTime;
        CBrain.m_DefaultBlend.m_Style = _style;
        Cameras[CurrentCamera].m_Lens.OrthographicSize = _res;

        Cameras[CurrentCamera].Priority = 11;
        Cameras[FlipNum(CurrentCamera)].Priority = 10;
    }

    public void ZoomViaOrig(float _multiplyer, float _blendTime,
    CinemachineBlendDefinition.Style _style = CinemachineBlendDefinition.Style.EaseOut)
    {
        Zoom(OriginalRes * _multiplyer, _blendTime, _style);
    }

// Set original resolution
    public void SetOrigRes(float _res)
    { OriginalRes = _res; }


// Set current resolution to original resolution
    public void ReturntoOrigRes(float _blendTime,
    CinemachineBlendDefinition.Style _style = CinemachineBlendDefinition.Style.EaseOut)
    {
        Zoom(OriginalRes, _blendTime, _style);
    }
}
