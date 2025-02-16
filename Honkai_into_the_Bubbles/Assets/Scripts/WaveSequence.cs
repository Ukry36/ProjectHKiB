using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave
{
    public float spawnInterval;
    public float waveTimeInterval;

    [System.Serializable]
    public class MonsterInfo
    {
        public GameObject monsterPrefab;
        public int count;
    }

    public List<MonsterInfo> monsters = new List<MonsterInfo>();
}

public class WaveSequence : MonoBehaviour
{
    public List<Wave> frontWaves;
    public List<Wave> middleWaves;
    public List<Wave> backWaves;
}
