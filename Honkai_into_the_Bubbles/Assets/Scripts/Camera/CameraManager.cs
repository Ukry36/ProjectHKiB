using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CameraManager : MonoBehaviour
{
    #region Singleton
    static public CameraManager instance;
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

    [SerializeField] private CinemachineVirtualCamera[] Cameras = new CinemachineVirtualCamera[2];
    private CinemachineConfiner2D[] Confiners = new CinemachineConfiner2D[2];
    [SerializeField] private CinemachineBrain CBrain;

    [SerializeField] private BGRenderer bgrenderer;

    public float OriginalRes = 5;
    private int CurrentCamera = 0; // 0 or 1
    public bool freeze = false;

    private void Start()
    {
        this.transform.position = PlayerManager.instance.transform.position;
        for (int i = 0; i < Cameras.Length; i++)
        {
            Confiners[i] = Cameras[i].GetComponent<CinemachineConfiner2D>();
        }
        Cameras[CurrentCamera].Priority = 11;
        ReturntoOrigRes(0);
    }

    public void StrictMovement(Vector3 _pos, Vector3 _prevPos)
    {
        this.transform.position = _pos;
        for (int i = 0; i < Cameras.Length; i++)
        {
            Cameras[i].OnTargetObjectWarped(PlayerManager.instance.transform, _pos - _prevPos);
        }
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CameraBound"))
        {
            if (other.TryGetComponent(out AreaInfo areaInfo))
            {
                for (int i = 0; i < Cameras.Length; i++)
                {
                    Confiners[i].m_BoundingShape2D = areaInfo.cameraBound;
                }


                bgrenderer.RenderBackGround(areaInfo.backGround);


                AudioManager.instance.ChangeAreaBGMs(areaInfo.areaBGMs, areaInfo.fadeTime);


                WhetherManager.instance.ChangeAreaWhethers(areaInfo.areaWhetherTypes, true);
            }
        }
    }
}
