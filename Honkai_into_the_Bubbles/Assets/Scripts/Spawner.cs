using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject waveManager;

    private bool waveStarted = false;

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
}
