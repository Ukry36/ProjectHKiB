using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject waveManager;

    private bool waveStarted = false;

    private void Start()
    {
        Debug.Log("Spawner enabled");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trgger entered");
        if (collision != null)
        {
            if (collision.CompareTag("Player") && !waveStarted)
            {
                Debug.Log("player entered");
                if (waveManager != null)
                {
                    waveManager.SetActive(true);
                    waveStarted = true;
                    Debug.Log("manager enabled");
                }
                else
                {
                    Debug.Log("reference is null");
                }
            }
        }
    }

}
