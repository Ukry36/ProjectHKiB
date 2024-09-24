using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;

public class SpawnFieldEvent : Event
{
    [SerializeField] private GameObject Prefab;
    [SerializeField] private int ID = 30000;
    [SerializeField] private float checkOffset;
    [SerializeField] private Vector3 TR;
    [SerializeField] private Vector3 BL;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask spawnLayer;
    [SerializeField] private int countToMaintain;

    private List<Vector3> spawnPoints = new();

    protected override void StartEvent(Status _interactedEntity)
    {
        StartCoroutine(Cooltime());
        int fillCount = GetFillCount();

        for (int i = 0; i < fillCount; i++)
            GetRandomPos();
        for (int i = 0; i < spawnPoints.Count; i++)
            PoolManager.instance.ReuseGameObject(Prefab, spawnPoints[i], quaternion.identity);
        spawnPoints = new();
        EndEvent();
    }

    private int GetFillCount()
    {
        List<Collider2D> colliders = new(Physics2D.OverlapAreaAll(TR, BL, spawnLayer));
        List<Collider2D> outColliders = new();
        for (int i = 0; i < colliders.Count; i++)
        {

            if (colliders[i].TryGetComponent(out Status status))
            {
                if (status.entity.ID == ID)
                    outColliders.Add(colliders[i]);
            }
        }
        return outColliders.Count < countToMaintain ? countToMaintain - outColliders.Count : 0;
    }

    private void GetRandomPos()
    {
        Vector3 outputPos;
        for (int i = 0; i < 5; i++)
        {
            outputPos = Vector3Int.FloorToInt(new Vector3(UnityEngine.Random.Range(BL.x, TR.x), UnityEngine.Random.Range(BL.y, TR.y)));
            outputPos += new Vector3(checkOffset, checkOffset);
            if (!Physics2D.OverlapCircle(outputPos, 0.5f, wallLayer) && !spawnPoints.Contains(outputPos))
            {
                spawnPoints.Add(outputPos);
                return;
            }
        }
    }

    protected override IEnumerator Cooltime()
    {
        isCooltime = true;
        yield return new WaitForSeconds(cooltime);
        isCooltime = false;
    }
}
