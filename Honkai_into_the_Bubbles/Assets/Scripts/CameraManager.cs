using System.Collections;
using System.Diagnostics;
using Cinemachine;
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
    
    [SerializeField] private CinemachineVirtualCamera originalCamera;
    public bool freeze = false;

    private void Start()
    {
        originalCamera.Priority = 11;
    }

    public void Zoom(int _i)
    {
        switch(_i)
        {
            case 0:
                
            break;
        }
    }
}
