using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager2 : MonoBehaviour
{
    [SerializeField] private List<GameObject> beforeObjects = new();
    [SerializeField] private List<GameObject> afterObjects = new();
    public WaveSequence WaveSequence;
    public WaveTileManager2 waveTileManager2;
    public WaveVisualInfoManager waveVisualManager;
    public List<Wave> currentWaves;

    private int currentWaveIndex = 0;
    private int aliveMonsters = 0;

    private bool spawnInProgress = false;
    private bool setTileInProgress = false;
    private bool transitionInProgress = false;

    private float transitionDuration = 1f;
    [SerializeField] private float checkOffset;
    [SerializeField] private Vector3 TR;
    [SerializeField] private Vector3 BL;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask spawnLayer;

    public GameObject AfterGrid;
    public GameObject BeforeGrid;

    private List<Vector3> spawnPoints = new();
    private enum WaveState
    {
        Init,
        Spawning,
        Waiting,
        SetTile,
        Transition,
        End
    }

    private WaveState currentState = WaveState.Init;

    private void Start()
    {
        currentWaves = WaveSequence.frontWaves;
        waveTileManager2.waveSequence = WaveSequence;
        waveTileManager2.OnTileSetCompleted += OnTileSetCompleted;
        waveVisualManager.OnWaveTransition += OnLightTransitionComplete;

        foreach (var gameObject in beforeObjects)
        {
            gameObject.SetActive(true);
        }

        foreach (var gameObject in afterObjects)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable() => waveVisualManager.BeforeToFrontTransition();

    private void OnDisable() => waveVisualManager.RearToAfterTransition();

    public void GridChange()
    {
        AfterGrid.SetActive(true);
        BeforeGrid.SetActive(false);
    }

    private void Update()
    {
        switch (currentState)
        {
            case WaveState.Init: // Init
                foreach (var gameObject in beforeObjects)
                    gameObject.SetActive(false);
                currentState = WaveState.Spawning;
                break;


            case WaveState.Spawning: // Start spawning hostiles
                if (!spawnInProgress && currentWaveIndex < currentWaves.Count)
                    StartCoroutine(SpawnWave(currentWaves[currentWaveIndex]));

                currentState = WaveState.Waiting;
                break;


            case WaveState.Waiting: // Wait until player kills all hostiles and setTile ends
                Debug.Log(setTileInProgress);
                if (!spawnInProgress && !setTileInProgress && aliveMonsters < 1)
                    currentState = WaveState.SetTile;
                break;


            case WaveState.SetTile: // Start setTile and manage an end of wave
                if (currentWaves != WaveSequence.middleWaves)
                    SetTile();

                currentWaveIndex++;

                if (currentWaveIndex >= currentWaves.Count)
                    currentState = WaveState.Transition;
                else
                    currentState = WaveState.Spawning;
                break;


            case WaveState.Transition: // If wave is entering or exitting middle wave, do transition
                if (!transitionInProgress)
                    WaveTransition();
                break;


            case WaveState.End: // End waves
                foreach (var gameObject in afterObjects)
                    gameObject.SetActive(true);
                this.gameObject.SetActive(false);
                break;
        }
    }

    void OnTileSetCompleted() => setTileInProgress = false;

    private void SetTile()
    {
        setTileInProgress = true;

        if (currentWaves == WaveSequence.frontWaves)
            waveTileManager2.FrontWaveCompleted(currentWaveIndex);
        else if (currentWaves == WaveSequence.backWaves)
            waveTileManager2.BackWaveCompleted(currentWaveIndex);
    }

    IEnumerator SpawnWave(Wave wave)
    {
        spawnInProgress = true;

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
        spawnInProgress = false;

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
        transitionInProgress = true;
        if (currentWaves == WaveSequence.frontWaves)
        {
            waveVisualManager.FrontToMiddleTransition(transitionDuration, 20);
            currentWaves = WaveSequence.middleWaves;
            GridChange();
        }
        else if (currentWaves == WaveSequence.middleWaves)
        {
            currentWaves = WaveSequence.backWaves;
            waveVisualManager.MiddleToRearTransition(transitionDuration, 20);
        }
        else if (currentWaves == WaveSequence.backWaves)
        {
            currentState = WaveState.End;
        }

        currentWaveIndex = 0;
    }

    private void OnLightTransitionComplete()
    {
        transitionInProgress = false;
        currentState = WaveState.Spawning;
    }
}
