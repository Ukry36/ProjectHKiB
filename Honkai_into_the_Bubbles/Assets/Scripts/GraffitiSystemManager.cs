using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GPath
{
    public Vector3 pos;
    public int GTPCCI;

    public GPath(Vector3 _pos, int _GTPCCI)
    {
        pos = _pos;
        GTPCCI = _GTPCCI;
    }
}

public class GraffitiSystemManager : MonoBehaviour
{
    #region Singleton
    public static GraffitiSystemManager instance;

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
    [HideInInspector] public GraffitiSystem playerGS;
    [SerializeField] private GameObject GraffitiImpactPrefab;
    [SerializeField] private GameObject GraffitiPathPrefab;
    [SerializeField] private float pathIntervalTime = 0.05f;
    [SerializeField] private bool pathReverse = true;
    [HideInInspector] public int totalGraffitiCount = 0;
    private List<Color> colors;
    private List<GPath> Paths;

    public void OnGraffitiStart()
    {
        Paths = new();
        this.transform.position = playerGS.transform.position;
        totalGraffitiCount++;
        colors = PlayerManager.instance.ThemeColors;
        playerGS.AddTile(Vector2.zero);
        GraffitiImpact();
    }

    public void GraffitiImpact()
    {
        GameObject clone = PoolManager.instance.ReuseGameObject(GraffitiImpactPrefab, this.transform.position, quaternion.identity);
        ParticleSystem.MainModule PS = clone.GetComponent<ParticleSystem>().main;
        PS.startColor = colors[(totalGraffitiCount + 1) % colors.Count];
    }

    public void GraffitiPath(Vector3 _pos, int _GTPCCI)
    {
        Paths.Add(new GPath(_pos, _GTPCCI));
    }

    public void OnGraffitiEnd()
    {
        StartCoroutine(OnGraffitiEndCoroutine(pathIntervalTime, pathReverse));
    }

    private IEnumerator OnGraffitiEndCoroutine(float _interval, bool _reverse)
    {
        List<GPath> paths = Paths;
        for (int i = 0; i < paths.Count; i++)
        {
            int j = _reverse ? paths.Count - 1 - i : i;
            GameObject clone = PoolManager.instance.ReuseGameObject(GraffitiPathPrefab, paths[j].pos, quaternion.identity);
            ParticleSystem.MainModule PS = clone.GetComponent<ParticleSystem>().main;
            PS.startColor = colors[(totalGraffitiCount + paths[j].GTPCCI) % colors.Count];
            yield return new WaitForSeconds(_interval);
        }
    }
}
