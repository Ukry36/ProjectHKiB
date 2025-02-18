using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;

public class SpawnEvent : Event
{
    [SerializeField] private GameObject Prefab;
    [SerializeField] private int ID = 30000;
    [SerializeField] private Vector3 TR;
    [SerializeField] private Vector3 BL;
    [SerializeField] private LayerMask wallLayer;

    private Vector3 spawnPoint;
    private int size;

    protected void Start()
    {
        size = Prefab.GetComponent<Status>().Size;
    }
    protected override void StartEvent(Status _interactedEntity)
    {
        StartCoroutine(Cooltime());
        if (GetFillCount() == 1)
        {
            if (GetRandomPos())
                PoolManager.instance.ReuseGameObject(Prefab, spawnPoint, quaternion.identity);
        }

        spawnPoint = new();
        EndEvent();
    }

    private int GetFillCount() => PoolManager.instance.GetEntityNum(ID) < 40 ? 1 : 0;

    private bool GetRandomPos()
    {
        float checkOffset = (float)size / 2 - 0.5f;
        for (int i = 0; i < 5; i++)
        {
            spawnPoint = Vector3Int.FloorToInt(new Vector3(UnityEngine.Random.Range(BL.x, TR.x), UnityEngine.Random.Range(BL.y, TR.y)));
            spawnPoint += new Vector3(checkOffset, checkOffset);
            if (!Physics2D.OverlapCircle(spawnPoint, 0.5f, wallLayer))
                return true;
        }
        return false;
    }

    protected override IEnumerator Cooltime()
    {
        isCooltime = true;
        yield return new WaitForSeconds(cooltime);
        isCooltime = false;
    }
}
