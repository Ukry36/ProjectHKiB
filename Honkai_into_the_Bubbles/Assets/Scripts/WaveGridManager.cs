using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaveGridManager : MonoBehaviour
{
    public GameObject AfterGrid;
    public GameObject BeforeGrid;

    public void GridChange()
    {
        AfterGrid.SetActive(true);
        BeforeGrid.SetActive(false);
    }
}
