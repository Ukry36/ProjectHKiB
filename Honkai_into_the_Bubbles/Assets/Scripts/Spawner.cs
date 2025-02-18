using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject waveManager;

    private bool waveStarted = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Player") && !waveStarted)
            {
                if (waveManager != null && InputManager.instance.ConfirmInput)
                {
                    waveManager.SetActive(true);
                    waveStarted = true;
                }
            }
        }
    }
}
