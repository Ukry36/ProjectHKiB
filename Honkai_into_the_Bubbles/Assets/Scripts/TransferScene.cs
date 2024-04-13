using System.Collections;
using UnityEngine;

public class TransferScene : TransferPosition
{
    [SerializeField] private string destinationSceneName;

    protected override IEnumerator TransferCoroutine(State component)
    {
        yield return SceneLoadingManager.instance.LoadSceneCoroutine(
            destinationSceneName, fadeColor, delay, innerDelay, component, destination.position - this.transform.position);
    }

}