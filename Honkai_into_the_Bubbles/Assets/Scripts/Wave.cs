using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int waveNumber;
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

