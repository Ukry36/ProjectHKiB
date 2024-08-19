using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;

public class SpawnFieldEvent : Event
{
    [SerializeField] private GameObject Prefab;
    private int ID;
    [SerializeField] private float checkOffset;
    [SerializeField] private Vector3 TR;
    [SerializeField] private Vector3 BL;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask spawnLayer;
    [SerializeField] private bool checkFieldEnemyCount;
    [ShowIf("checkFieldEnemyCount")][SerializeField] private int countToMaintain;

    private List<Vector3> spawnPoints = new();

    private void Awake()
    {
        ID = Prefab.GetComponent<Status>().entity.ID;
    }
    protected override void StartEvent(Status _interactedEntity)
    {
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
        if (colliders.Count > 0)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out Status status))
                    if (status.entity.ID != ID)
                        colliders.Remove(collider);
            }
        }
        return colliders.Count < countToMaintain ? countToMaintain - colliders.Count : 0;
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
}
