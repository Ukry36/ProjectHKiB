using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishEffect : MonoBehaviour
{
    [SerializeField] private GameObject[] VanishPrefab;
    private void OnDestroy()
    {
        var clone = Instantiate(VanishPrefab[Random.Range(0, VanishPrefab.Length)], this.transform.position, Quaternion.identity);
        clone.SetActive(true);
    }
}
