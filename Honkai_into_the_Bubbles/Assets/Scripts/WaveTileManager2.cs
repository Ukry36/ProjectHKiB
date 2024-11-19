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
    public float objectProbability = 0.1f;

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
    public delegate void TileSetCompleted();
    public event TileSetCompleted OnTileSetCompleted;
    public GameObject objectPrefab;

    private List<FieldElement> TileField;
    private List<List<FieldElement>> SlicedTileFields;

    [SerializeField] private AnimationCurve XCurve;
    [SerializeField] private AnimationCurve YCurve;

    [SerializeField][MaxValue(1), MinValue(0)] private float shuffle;
    [SerializeField][MinValue(1)] private int StepPerWave;
    [SerializeField][MinValue(0)] private float delayPerStep;
    [SerializeField][MaxValue(1), MinValue(0)] private float stepSpeed;

    private const bool RESTORE = true;
    private const bool REMOVE = false;

    void Start()
    {
        InitField();
    }

    private void InitField()
    {
        TileField = new();
        BoundsInt bounds = tilemap.cellBounds;

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
                }
            }
        }

        TileField.Sort((a, b) => b.probability.CompareTo(a.probability));

        float max = TileField[0].probability;
        foreach (var comp in TileField)
        {
            comp.probability /= max + 0.0001f;
        }

        TileField = ProbShuffle(TileField, (int)(TileField.Count * shuffle));
        TileField.Sort((a, b) => b.probability.CompareTo(a.probability));

        tilemap.ClearAllTiles();
    }

    private List<FieldElement> ProbShuffle(List<FieldElement> _field, int _strength)
    {
        for (int i = 0; i < _field.Count; i++)
        {
            int pos = i + UnityEngine.Random.Range(-_strength, _strength);
            pos = pos < 0 ? 0 : pos > _field.Count - 1 ? _field.Count - 1 : pos;

            (_field[i].probability, _field[pos].probability) = (_field[pos].probability, _field[i].probability);
        }
        return _field;
    }

    private void InitDiscrete(bool _setOrRemove)
    {
        int WaveCount = _setOrRemove ? waveSequence.frontWaves.Count : waveSequence.backWaves.Count;
        SlicedTileFields = new();

        for (int i = 0; i < WaveCount; i++)
        {
            SlicedTileFields.Insert(0, TileField.FindAll(a => Mathf.FloorToInt(a.probability * WaveCount) == i));
        }
    }

    private IEnumerator SetOneWaveTiles(int _waveIndex, bool _setOrRemove)
    {
        int tilePerStep = SlicedTileFields[_waveIndex].Count / StepPerWave;

        for (int i = 0; i < StepPerWave; i++)
        {
            int div = (int)(tilePerStep * stepSpeed);
            div = div < 1 ? 1 : div;
            for (int j = 0; j < tilePerStep; j++)
            {
                FieldElement tileElement = SlicedTileFields[_waveIndex][0];
                tilemap.SetTile(tileElement.pos, _setOrRemove ? tileElement.tile : null);

                if (UnityEngine.Random.value < tileElement.objectProbability)
                {
                    Instantiate(objectPrefab, tilemap.GetCellCenterWorld(tileElement.pos), Quaternion.identity);
                }
                SlicedTileFields[_waveIndex].Remove(tileElement);

                if (j % div == 0)
                    yield return null;
            }
            if (i == StepPerWave - 1)
                foreach (var tile in SlicedTileFields[_waveIndex])
                {
                    tilemap.SetTile(tile.pos, _setOrRemove ? tile.tile : null);
                    if (UnityEngine.Random.value < tile.objectProbability)
                    {
                        Instantiate(objectPrefab, tilemap.GetCellCenterWorld(tile.pos), Quaternion.identity);
                    }
                }
            yield return new WaitForSeconds(delayPerStep);
        }

        OnTileSetCompleted?.Invoke();
    }

    public void FrontWaveCompleted(int waveIndex)
    {
        InitDiscrete(RESTORE);
        StartCoroutine(SetOneWaveTiles(waveIndex, RESTORE));
    }

    public void BackWaveCompleted(int waveIndex)
    {
        InitDiscrete(REMOVE);
        StartCoroutine(SetOneWaveTiles(waveIndex, REMOVE));
    }
}