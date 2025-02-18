using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
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

    private float transitionDuration = 0.36f;
    [SerializeField] private Vector3 TR;
    [SerializeField] private Vector3 BL;
    [SerializeField] private LayerMask wallLayer;

    private Vector3 spawnPoint;
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

    private void OnEnable()
    {
        waveTileManager2.waveSequence = WaveSequence;
        waveVisualManager.BeforeToFrontTransition();
    }

    private void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded) return;
        waveVisualManager.RearToAfterTransition();
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
                if (!transitionInProgress && !setTileInProgress)
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
            int size = monsterInfo.monsterPrefab.GetComponent<Status>().Size;
            for (int i = 0; i < monsterInfo.count; i++)
            {
                if (GetRandomPos(size))
                {
                    GameObject monster = PoolManager.instance.ReuseGameObject(monsterInfo.monsterPrefab, spawnPoint, Quaternion.identity);
                    Status status = monster.GetComponent<Status>();
                    if (status != null)
                    {
                        OnMonsterSpawned(status);
                    }
                    yield return new WaitForSeconds(wave.spawnInterval);
                }

            }
        }

        spawnPoint = new();
        spawnInProgress = false;

        yield return new WaitForSeconds(wave.waveTimeInterval);
    }

    private bool GetRandomPos(int _size)
    {
        float checkOffset = (float)_size / 2 - 0.5f;
        for (int i = 0; i < 5; i++)
        {
            spawnPoint = Vector3Int.FloorToInt(new Vector3(UnityEngine.Random.Range(BL.x, TR.x), UnityEngine.Random.Range(BL.y, TR.y)));
            spawnPoint += new Vector3(checkOffset, checkOffset);
            if (!Physics2D.OverlapCircle(spawnPoint, 0.5f, wallLayer))
                return true;
        }
        return false;
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
