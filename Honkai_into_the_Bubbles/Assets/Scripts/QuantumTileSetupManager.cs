using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantumTileSetupManager : MonoBehaviour
{
    public int[,] qArray;


    public void GetQArray(int[,] _array)
    {
        qArray = _array;
    }

    public void FillArray(float _prop, bool _isFill)
    {
        int fillValue = _isFill ? 1 : 0;
        int totalCells = qArray.GetLength(0) * qArray.GetLength(1);
        int cellsToFill = (int)(totalCells * _prop);

        for (int i = 0; i < cellsToFill; i++)
        {
            int x, y;
            do
            {
                x = Random.Range(0, qArray.GetLength(0));
                y = Random.Range(0, qArray.GetLength(1));
            }
            while (qArray[x, y] == fillValue); 

            qArray[x, y] = fillValue;
        }
    }
}
