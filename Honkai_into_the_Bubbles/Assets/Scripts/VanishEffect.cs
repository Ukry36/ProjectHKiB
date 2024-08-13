using UnityEngine;

public class VanishEffect : MonoBehaviour
{
    [SerializeField] private GameObject[] VanishPrefab;
    private void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded) return;
        var clone = Instantiate(VanishPrefab[Random.Range(0, VanishPrefab.Length)], this.transform.position, Quaternion.identity);
        clone.SetActive(true);
    }
}
