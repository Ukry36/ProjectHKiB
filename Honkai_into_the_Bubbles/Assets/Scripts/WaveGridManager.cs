using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaveGridManager : MonoBehaviour
{
    public GameObject ChangeGrid;
    
    public void GridChange()
    {
        ChangeGrid.SetActive(true);
    }
}
