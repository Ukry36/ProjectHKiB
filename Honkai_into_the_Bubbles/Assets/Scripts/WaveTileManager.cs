using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class WaveTileManager : MonoBehaviour
{
    public Tilemap tilemap;
    public WaveSequence waveSequence;
    public WaveManager waveManager;
    public BoundsInt particularTile;
    private List<Vector3Int> tilePositions;  // List for save tilePosition
    private Dictionary<Vector3Int, TileBase> originalTiles; //original Tile status
    private int totalTiles;
    private int tilesPerWave;
    private float[] columnProbabilities;
    private float[] initialProbabilities;

    public delegate void TileSetCompleted();
    public event TileSetCompleted OnTileSetCompleted;

    void Start()
    {
        InitializeTilePositions();
        ClearTilemap();
    }

    private void InitializeTilePositions()
    {
        BoundsInt bounds = tilemap.cellBounds;
        tilePositions = new List<Vector3Int>();
        originalTiles = new Dictionary<Vector3Int, TileBase>();
        totalTiles = bounds.size.x * bounds.size.y;

        columnProbabilities = new float[bounds.size.x];
        float probabilityStep = 0.8f / (bounds.size.x - 1);

        for (int i = 0; i < bounds.size.x; i++)
        {
            columnProbabilities[i] = 0.7f - (i * probabilityStep);
        }

        initialProbabilities = (float[])columnProbabilities.Clone();

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                tilePositions.Add(pos);
                TileBase tile = tilemap.GetTile(pos);
                Debug.Log(pos);
                if (tile != null)
                {
                    originalTiles[pos] = tile;
                }
            }
        }

        tilesPerWave = Mathf.FloorToInt(totalTiles / waveSequence.frontWaves.Count);
    }

    void SaveOriginalTiles()
    {
        foreach (var pos in tilePositions)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile != null)
            {
                originalTiles[pos] = tile;
            }
        }
    }

    private void ClearTilemap()
    {
        tilemap.ClearAllTiles();
    }

    List<Vector3Int> GetTilePositionsInBounds(BoundsInt bounds)
    {
        List<Vector3Int> positions = new List<Vector3Int>();

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (originalTiles.ContainsKey(pos))
                {
                    positions.Add(pos);
                }
            }
        }

        return positions;
    }

    private IEnumerator RestoreTiles(int waveIndex)
    {
        int totalWaves = waveSequence.frontWaves.Count;
        int tilesToRestore = Mathf.FloorToInt(totalTiles / totalWaves);

        if (waveIndex == totalWaves - 1)
        {
            List<Vector3Int> unrecoveredTiles = tilePositions.FindAll(pos => tilemap.GetTile(pos) == null);
            foreach (var pos in unrecoveredTiles)
            {
                if (originalTiles.ContainsKey(pos))
                {
                    tilemap.SetTile(pos, originalTiles[pos]);

                    yield return new WaitForSeconds(0.001f);
                }
            }
            OnTileSetCompleted?.Invoke();
        }

        int restoredTiles = 0;
       
        List<Vector3Int> availablePositions = new List<Vector3Int>(tilePositions);
        availablePositions.RemoveAll(pos => tilemap.GetTile(pos) != null);

        foreach (var tilePos in tilePositions)
        {
            if (restoredTiles >= tilesToRestore)
            {
                break;
            }

            int columnIndex = tilePos.x - tilemap.cellBounds.xMin;
            if (columnIndex >= 0 && columnIndex < columnProbabilities.Length)
            {
                float probability = columnProbabilities[columnIndex];
                if (Random.value <= probability && originalTiles.ContainsKey(tilePos) && tilemap.GetTile(tilePos) == null)
                {
                    tilemap.SetTile(tilePos, originalTiles[tilePos]);
                    restoredTiles++;

                    yield return new WaitForSeconds(0.001f);
                }
            }
            else if(columnIndex < 0 && columnIndex < columnProbabilities.Length)
            {
                int k = 0;
                float probability = columnProbabilities[k];
                if (Random.value <= probability && originalTiles.ContainsKey(tilePos) && tilemap.GetTile(tilePos) == null)
                {
                    tilemap.SetTile(tilePos, originalTiles[tilePos]);
                    restoredTiles++;

                    yield return new WaitForSeconds(0.001f);
                }
                k++;
            }
        }
        ShiftArrayRight(columnProbabilities, 12);
        OnTileSetCompleted?.Invoke();
    }

    private IEnumerator RemoveTiles(int waveIndex)
    {
        Debug.Log("remove");
        int totalWaves = waveSequence.backWaves.Count;
        int tilesToRemove = Mathf.FloorToInt(totalTiles / totalWaves);

        if (waveIndex == totalWaves - 1)
        {
            /*
            List<Vector3Int> shuffledPositions = new List<Vector3Int>(tilePositions); // 완전 무작위 버전
            for (int i = 0; i < shuffledPositions.Count; i++)
            {
                Vector3Int temp = shuffledPositions[i];
                int randomIndex = Random.Range(i, shuffledPositions.Count);
                shuffledPositions[i] = shuffledPositions[randomIndex];
                shuffledPositions[randomIndex] = temp;
            }

            foreach (var pos in shuffledPositions)
            {
                if (originalTiles.ContainsKey(pos) && tilemap.GetTile(pos) != null)
                {
                    tilemap.SetTile(pos, null);
                    yield return new WaitForSeconds(0.001f);
                }
            }
            */
            bool allTilesRemoved = false;
            float removalChance = 0.6f; // 첫 번째 패스에서 타일을 제거할 확률

            while (!allTilesRemoved)
            {
                allTilesRemoved = true;

                foreach (var pos in tilePositions)
                {
                    if (originalTiles.ContainsKey(pos) && tilemap.GetTile(pos) != null)
                    {
                        if (Random.value <= removalChance)
                        {
                            tilemap.SetTile(pos, null);
                            yield return new WaitForSeconds(0.001f);
                        }
                        else
                        {
                            allTilesRemoved = false;
                        }
                    }
                }

                removalChance = Mathf.Min(removalChance + 0.1f, 1.0f);
            }
            OnTileSetCompleted?.Invoke();
            yield break;
        }

        int removeTiles = 0;

        List<Vector3Int> availablePositions = new List<Vector3Int>(tilePositions);
        availablePositions.RemoveAll(pos => tilemap.GetTile(pos) == null);

        foreach (var tilePos in tilePositions)
        {
            if (removeTiles >= tilesToRemove)
            {
                break;
            }

            int columnIndex = tilePos.x - tilemap.cellBounds.xMin;
            if (columnIndex >= 0 && columnIndex < initialProbabilities.Length)
            {
                float probability = initialProbabilities[columnIndex];
                if (Random.value <= probability && originalTiles.ContainsKey(tilePos) && tilemap.GetTile(tilePos) != null)
                {
                    tilemap.SetTile(tilePos, null);
                    removeTiles++;

                    yield return new WaitForSeconds(0.001f);
                }
            }
            else if (columnIndex < 0 && columnIndex < initialProbabilities.Length)
            {
                int k = 0;
                float probability = initialProbabilities[k];
                if (Random.value <= probability && originalTiles.ContainsKey(tilePos) && tilemap.GetTile(tilePos) != null)
                {
                    tilemap.SetTile(tilePos, null);
                    removeTiles++;

                    yield return new WaitForSeconds(0.001f);
                }
                k++;
            }
        }
        ShiftArrayRight(initialProbabilities, 12);
        OnTileSetCompleted?.Invoke();
        yield break;
    }
    public void FrontWaveCompleted(int waveIndex)
    {
        StartCoroutine(RestoreTiles(waveIndex));
    }

    public void BackWaveCompleted(int waveIndex)
    {
        StartCoroutine(RemoveTiles(waveIndex));
    }

    public static void ShiftArrayRight<T>(T[] array, int shiftAmount)
    {
        int length = array.Length;
        T[] tempArray = new T[length];

        for (int i = 0; i < length; i++)
        {
            tempArray[(i + shiftAmount) % length] = array[i];
        }

        for (int i = 0; i < length; i++)
        {
            array[i] = tempArray[i];
        }
    }


}