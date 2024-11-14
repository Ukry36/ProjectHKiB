using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject waveManager;

    private bool waveStarted = false;

    private void Start()
    {
        waveManager.GetComponent<WaveManager2>().OnWaveEnd += OnWaveEnd;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Player") && !waveStarted)
            {
                if (waveManager != null)
                {
                    waveManager.SetActive(true);
                    waveStarted = true;
                }
            }
        }
    }

    private void OnWaveEnd()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
