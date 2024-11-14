using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static WaveTileManager;

public class WaveManager : MonoBehaviour
{
    public WaveSequence WaveSequence;
    public List<Wave> currentWaves;
    [SerializeField]
    private int currentWaveIndex = 0;
    [SerializeField]
    private int aliveMonsters = 0;
    private int monsterCount = 0;
    private int rusherCount = 0;
    private int spiderCount = 0;
    private bool waveInProgress = false;
    private bool isFrontWaves = true;

    [SerializeField] private GameObject Prefab;
    [SerializeField] private GameObject Prefab2;
    [SerializeField] private float checkOffset;
    [SerializeField] private Vector3 TR;
    [SerializeField] private Vector3 BL;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask spawnLayer;

    public WaveTileManager waveTileManager;

    private List<Vector3> spawnPoints = new();
    private enum WaveState
    {
        Idle,
        Spawning,
        Cooldown,
        Restoring,
        Transition
    }

    private WaveState currentState = WaveState.Idle;

    private void Start()
    {
        currentWaves = WaveSequence.frontWaves;

        waveTileManager.OnTileSetCompleted += OnTileSetCompleted;
    }

    private void OnEnable()
    {
        Debug.Log("manager is enabled");
    }
    private void Update()
    {
        switch (currentState)
        {
            case WaveState.Idle:
                if (!waveInProgress && currentWaveIndex < currentWaves.Count)
                {
                    StartCoroutine(SpawnWave(currentWaves[currentWaveIndex]));
                }
                break;

            case WaveState.Spawning:
                break;

            case WaveState.Cooldown:
                if (!waveInProgress && aliveMonsters == 0)
                {
                    currentState = WaveState.Restoring;

                    if (waveTileManager != null)
                    {
                        if (currentWaves == WaveSequence.frontWaves)
                            waveTileManager.FrontWaveCompleted(currentWaveIndex);
                        else
                            waveTileManager.BackWaveCompleted(currentWaveIndex);
                    }

                    currentWaveIndex++;

                    if (currentWaveIndex >= currentWaves.Count)
                    {
                        if (isFrontWaves)
                        {
                            currentState = WaveState.Transition;
                        }
                        else
                        {
                            Debug.Log("All waves completed.");
                        }
                    }
                }
                break;

            case WaveState.Transition:
                isFrontWaves = false;
                currentWaveIndex = 0;
                currentWaves = WaveSequence.backWaves;
                Debug.Log("transition");
                break;

            case WaveState.Restoring:
                break;
        }
    }

    void OnTileSetCompleted()
    {
        currentState = WaveState.Idle;
    }

    IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log("wave start");
        rusherCount = wave.rusherCount;
        spiderCount = wave.spiderCount;
        monsterCount = rusherCount + spiderCount;
        waveInProgress = true;
        currentState = WaveState.Spawning;

        for (int i = 0; i < monsterCount; i++)
            GetRandomPos();
        for (int i = 0; i < rusherCount; i++)
        {
            GameObject monster = PoolManager.instance.ReuseGameObject(Prefab, spawnPoints[i], quaternion.identity);
            Status status = monster.GetComponent<Status>();
            if (status != null)
            {
                OnMonsterSpawned(status);
            }
        }
        for (int i = 0; i < spiderCount; i++)
        {
            GameObject monster2 = PoolManager.instance.ReuseGameObject(Prefab2, spawnPoints[i + rusherCount], quaternion.identity);
            Status status2 = monster2.GetComponent<Status>();
            if (status2 != null)
            {
                OnMonsterSpawned(status2);
            }
        }
        spawnPoints = new();
        waveInProgress = false;
        currentState = WaveState.Cooldown;

        yield return new WaitForSeconds(wave.waveTimeInterval);
    }

    private void GetRandomPos()
    {
        Debug.Log("pos calculated");
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

    public void OnMonsterSpawned(Status status)
    {
        aliveMonsters++;

        if (status.entity != null)
        {
            status.entity.OnDeath -= OnMonsterDeath; // 이전 등록 해제
            status.entity.OnDeath += OnMonsterDeath; // 새로운 등록
        }
    }

    public void OnMonsterDeath(Entity entity)
    {
        aliveMonsters--;
        entity.OnDeath -= OnMonsterDeath;
    }
}
