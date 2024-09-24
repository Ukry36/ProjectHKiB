using System.Collections;
using UnityEngine;

public class TransferScene : TransferPosition
{
    [SerializeField] private string destinationSceneName;

    protected override IEnumerator TransferCoroutine(Status _interactedEntity)
    {
        yield return TeleportManager.instance.LoadSceneCoroutine(
            destinationSceneName, _interactedEntity, dir, destination.position, delay, innerDelay, fadeColor);
        EndEvent();
    }

}