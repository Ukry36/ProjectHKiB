using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraffitiSystemManager : MonoBehaviour
{
    #region Singleton
    public static GraffitiSystemManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            prevPos = this.transform.position + Vector3.one;
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    [HideInInspector] public GraffitiSystem playerGS;
    public GameObject GraffitiTilePrefab;
    private Vector3 prevPos;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && prevPos != other.transform.position)
        {
            playerGS.AddTile(other.transform.position - this.transform.position);
            Instantiate(GraffitiTilePrefab, other.transform.position, Quaternion.identity);
            prevPos = other.transform.position;
        }
    }
}
