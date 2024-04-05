using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferScene : MonoBehaviour
{
    [SerializeField] private string destinationSceneName;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out MovePoint component))
        {
            if (component.Mover.name == "Player")
            {
                SceneManager.LoadScene(destinationSceneName);
            }
        }
    }
}