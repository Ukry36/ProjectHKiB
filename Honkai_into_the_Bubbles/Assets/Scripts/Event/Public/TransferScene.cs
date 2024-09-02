using System.Collections;
using UnityEngine;

public class TransferScene : TransferPosition
{
    [SerializeField] private string destinationSceneName;

    protected override IEnumerator TransferCoroutine(Status _interactedEntity)
    {
        yield return SceneLoadingManager.instance.LoadSceneCoroutine(
            destinationSceneName, fadeColor, delay, innerDelay, _interactedEntity, destination.position, _interactedEntity.entity.MovePoint.transform.position);
        EndEvent();
    }

}