using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class FieldElement
{
    public Vector3Int pos;
    public float probability;
    public TileBase tile;

    public FieldElement(Vector3Int _pos, float _probability, TileBase _tile)
    {
        this.pos = _pos;
        this.probability = _probability;
        this.tile = _tile;
    }
}

public class WaveTileManager2 : MonoBehaviour
{
    public Tilemap tilemap;
    public WaveSequence waveSequence;
    private int totalTileCount;
    public delegate void TileSetCompleted();
    public event TileSetCompleted OnTileSetCompleted;


    private List<FieldElement> TileField;
    private List<FieldElement> CompletedTileField;
    private List<FieldElement> AllignedTileField;

    [SerializeField] private AnimationCurve XCurve;
    [SerializeField] private AnimationCurve YCurve;

    [SerializeField] private int StepPerWave;
    [SerializeField] private float delayPerStep;
    [SerializeField][MaxValue(1), MinValue(0.01f)] private float stepSpeed;
    [SerializeField][MaxValue(1), MinValue(0.01f)] private float resolution;

    private const bool RESTORE = true;
    private const bool REMOVE = false;

    void Start()
    {
        InitField();
    }

    private void InitField()
    {
        TileField = new();
        CompletedTileField = new();

        BoundsInt bounds = tilemap.cellBounds;
        totalTileCount = 0;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new(x, y);
                TileBase tile = tilemap.GetTile(pos);
                if (tile != null)
                {
                    TileField.Add(new FieldElement
                    (
                        pos,
                        XCurve.Evaluate((float)(pos.x - bounds.xMin) / (bounds.xMax - bounds.xMin))
                        + YCurve.Evaluate((float)(pos.y - bounds.yMin) / (bounds.yMax - bounds.yMin)),
                        tile
                    ));
                    totalTileCount++;
                }
            }
        }

        tilemap.ClearAllTiles();
    }

    private void RestoreTileField()
    {
        foreach (var comp in CompletedTileField)
        {
            TileField.Add(comp);
        }
        CompletedTileField = new();
    }

    private void SetOneTileRandom()
    {
        for (float corr = 0; corr <= 1; corr += resolution)
        {
            FieldElement comp = TileField[UnityEngine.Random.Range(0, TileField.Count)];
            if (UnityEngine.Random.value < comp.probability + corr)
            {
                AllignedTileField.Add(comp);
                CompletedTileField.Add(comp);
                TileField.Remove(comp);
                break;
            }
        }
    }

    private IEnumerator AllignAndUpdateStep(bool _setOrRemove)
    {
        AllignedTileField.Sort((a, b) => b.probability.CompareTo(a.probability));
        int div = (int)(AllignedTileField.Count * stepSpeed);
        div = div < 1 ? 1 : div;
        // playsound
        for (int j = 0; j < AllignedTileField.Count; j++)
        {
            tilemap.SetTile(AllignedTileField[j].pos, _setOrRemove ? AllignedTileField[j].tile : null);
            if (j % div == 0)
                yield return null;
        }
        yield return new WaitForSeconds(delayPerStep);
    }

    private IEnumerator SetOneWaveTiles(int _waveIndex, bool _setOrRemove)
    {
        int WaveCount = _setOrRemove ? waveSequence.frontWaves.Count : waveSequence.backWaves.Count;
        int tilePerWave = Mathf.FloorToInt(totalTileCount / WaveCount);
        int tilePerStep = tilePerWave / StepPerWave;

        for (int i = 0; i < StepPerWave; i++)
        {
            AllignedTileField = new();

            for (int j = 0; j < tilePerStep; j++)
                SetOneTileRandom();

            yield return AllignAndUpdateStep(_setOrRemove);
        }

        if (_waveIndex >= WaveCount - 1 && TileField.Count > 0)
        {
            AllignedTileField = new();
            while (TileField.Count > 0)
                SetOneTileRandom();

            yield return AllignAndUpdateStep(_setOrRemove);
        }

        if (TileField.Count == 0)
        {
            // playsound
            RestoreTileField();
        }

        OnTileSetCompleted?.Invoke();
    }

    public void FrontWaveCompleted(int waveIndex)
    {
        StartCoroutine(SetOneWaveTiles(waveIndex, RESTORE));
    }

    public void BackWaveCompleted(int waveIndex)
    {
        StartCoroutine(SetOneWaveTiles(waveIndex, REMOVE));
    }
}