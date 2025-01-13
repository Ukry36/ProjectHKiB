using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WaveManager2 : MonoBehaviour
{
    public WaveSequence WaveSequence;
    public WaveGridManager waveGridManager;
    public WaveTileManager2 waveTileManager2;
    public LightManager lightManager;
    public List<Wave> currentWaves;
    public GameObject Tp;
    public delegate void WaveEnd();
    public event WaveEnd OnWaveEnd;

    private int currentWaveIndex = 0;
    private int aliveMonsters = 0;
    private bool waveInProgress = false;
    private bool isFrontWaves = true;
    private bool isMiddleWaves = false;
    private bool isBackWaves = false;
    private float transitionDuration = 5f;
    [SerializeField] private float checkOffset;
    [SerializeField] private Vector3 TR;
    [SerializeField] private Vector3 BL;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask spawnLayer;
    [SerializeField] private AreaInfo currentArea;
    private string currentWaveType = "frontwave";

    public float waveRes;
    public float afterRes;

    private List<Vector3> spawnPoints = new();
    private enum WaveState
    {
        Init,
        Idle,
        Spawning,
        Cooldown,
        Waiting,
        Transition,
        End
    }

    private WaveState currentState = WaveState.Init;

    private void Start()
    {
        currentWaves = WaveSequence.frontWaves;

        waveTileManager2.OnTileSetCompleted += OnTileSetCompleted;
        lightManager.OnWaveTransition += OnLightTransitionComplete;
    }

    private void OnEnable()
    {
        Debug.Log("manager is enabled");
        CameraManager.instance.SetOrigRes(waveRes);
        CameraManager.instance.ReturntoOrigRes(1f);
    }

    private void OnDisable()
    {
        CameraManager.instance.SetOrigRes(afterRes);
        CameraManager.instance.ReturntoOrigRes(1f);
    }

    private void Update()
    {
        switch (currentState)
        {
            case WaveState.Init:
                Tp.SetActive(false);
                currentState = WaveState.Idle;
                break;

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
                    currentState = WaveState.Waiting;

                    if (waveTileManager2 != null)
                    {
                        if (currentWaves == WaveSequence.frontWaves)
                            waveTileManager2.FrontWaveCompleted(currentWaveIndex);
                        else if (currentWaves == WaveSequence.backWaves)
                            waveTileManager2.BackWaveCompleted(currentWaveIndex);
                    }

                    if (currentWaves == WaveSequence.middleWaves)
                        currentState = WaveState.Idle;

                    currentWaveIndex++;

                    if (currentWaves == WaveSequence.middleWaves && currentWaveIndex >= currentWaves.Count)
                        currentState = WaveState.Transition;
                }
                break;

            case WaveState.Transition:
                WaveTransition();
                break;

            case WaveState.Waiting:
                break;

            case WaveState.End:
                Tp.SetActive(true);
                OnWaveEnd?.Invoke();
                break;
        }
    }

    void OnTileSetCompleted()
    {
        if (currentWaveIndex >= currentWaves.Count)
        {
            if (isFrontWaves)
            {
                Debug.Log("transition");
                currentState = WaveState.Transition;
            }
            else if (isBackWaves)
            {
                Debug.Log("end");
                currentState = WaveState.End;
            }
        }
        else
            currentState = WaveState.Idle;

    }

    IEnumerator SpawnWave(Wave wave)
    {
        waveInProgress = true;
        currentState = WaveState.Spawning;

        foreach (var monsterInfo in wave.monsters)
        {
            for (int i = 0; i < monsterInfo.count; i++)
            {
                GetRandomPos();
                GameObject monster = PoolManager.instance.ReuseGameObject(monsterInfo.monsterPrefab, spawnPoints[i], Quaternion.identity);
                Status status = monster.GetComponent<Status>();
                if (status != null)
                {
                    OnMonsterSpawned(status);
                }
                yield return new WaitForSeconds(wave.spawnInterval);
            }
        }

        spawnPoints = new();
        waveInProgress = false;
        currentState = WaveState.Cooldown;

        yield return new WaitForSeconds(wave.waveTimeInterval);
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

    public void OnMonsterSpawned(Status status)
    {
        aliveMonsters++;

        if (status.entity != null)
        {
            status.entity.OnDeath -= OnMonsterDeath;
            status.entity.OnDeath += OnMonsterDeath;
        }
    }

    public void OnMonsterDeath(Entity entity)
    {
        aliveMonsters--;
        entity.OnDeath -= OnMonsterDeath;
    }

    private void WaveTransition()
    {
        if (isFrontWaves)
        {
            Debug.Log("middlechange");
            isFrontWaves = false;
            isMiddleWaves = true;
            currentWaveType = "middleWave";
            lightManager.ActivateWaveLight(currentArea, currentWaveType, transitionDuration);
            currentWaves = WaveSequence.middleWaves;
            waveGridManager.GridChange();
        }
        else if (isMiddleWaves)
        {
            Debug.Log("backchange");
            isMiddleWaves = false;
            isBackWaves = true;
            currentWaveType = "backWave";
            currentWaves = WaveSequence.backWaves;
            lightManager.ActivateWaveLight(currentArea, currentWaveType, transitionDuration);
        }
        else if (isBackWaves)
        {
            currentWaveType = "frontWave";
            lightManager.ActivateWaveLight(currentArea, currentWaveType, transitionDuration);
        }

        currentWaveIndex = 0;
        currentState = WaveState.Waiting;
    }

    private void OnLightTransitionComplete()
    {
        currentState = WaveState.Idle;
    }
}
