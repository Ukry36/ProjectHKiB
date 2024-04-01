using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.Rendering;
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
    #endregion Singleton
    
    public GameObject target; // 카메라가 따라갈 대상
    public float moveSpeed; // 카메라 속도
    private Vector3 targetPosition; // 대상의 위치

    public BoxCollider2D bound;
    private Vector3 minBound;
    private Vector3 maxBound;

    private float halfWidth;
    private float halfHeight;
    // 위 둘을 구하기 위해 밑을 사용
    private Camera theCamera;
    private PixelPerfectCamera pixelPerfectCamera;
    public bool freeze = false;
    
    public int originalCameraResType = 3;
    public double[] CameraResolutionType = {2.416667, 2.9, 3.625, 4.833333333333, 7.25, 14.5};

    public int originalPixelHight = 180;

    private void Start()
    {
        theCamera = GetComponent<Camera>();
        pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * 16 / 9;
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(target.gameObject != null && !freeze)
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z); 

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);
            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
        }
    }

    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
    }

    public void Zoom(float _speed, double _res)
    {
        StopAllCoroutines();
        StartCoroutine(ZoomCoroutine(_speed, _res));
    }

    IEnumerator ZoomCoroutine(float _speed, double _res)
    {
        float tempf = pixelPerfectCamera.refResolutionY;
        if (_res < pixelPerfectCamera.refResolutionY)
        {
            while (_res < pixelPerfectCamera.refResolutionY)
            {
                yield return null;
                //theCamera.orthographicSize -= _speed * Time.deltaTime;
                tempf -= _speed * Time.deltaTime;
                pixelPerfectCamera.refResolutionY = (int)tempf;
                pixelPerfectCamera.refResolutionX = (int)tempf * 16 / 9 ;
                halfHeight = theCamera.orthographicSize;
                halfWidth = halfHeight * 16 / 9;
            }
        }
        else if (_res > pixelPerfectCamera.refResolutionY)
        {
            while (_res > pixelPerfectCamera.refResolutionY)
            {
                yield return null;
                //theCamera.orthographicSize += _speed * Time.deltaTime;
                tempf += _speed * Time.deltaTime;
                pixelPerfectCamera.refResolutionY = (int)tempf;
                pixelPerfectCamera.refResolutionX = (int)tempf * 16 / 9;
                halfHeight = theCamera.orthographicSize;
                halfWidth = halfHeight * 16 / 9;
            }
        }
        else
        {
            yield return null;
        }
        
    }
}
