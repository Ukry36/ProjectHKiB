using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    #region Singleton

    public static LayerManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public static LayerMask LayertoIgnore =
    (1 << LayerMask.NameToLayer("Ignore Raycast")) + (1 << LayerMask.NameToLayer("CameraBound"));
}
