using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveCustomizedObjectsTest : Event
{
    [SerializeField] private string removedSFX;
    [SerializeField] private ObjectCustomizeTest OCTest;


    protected override void StartEvent(Status _interactedEntity)
    {
        List<GameObject> gameObjects = OCTest.placedGameObjects;
        if (gameObjects.Count > 0) AudioManager.instance.PlaySoundFlat(removedSFX);

        for (int i = 0; i < gameObjects.Count; i++)
        {
            Destroy(gameObjects[i]);
        }
        OCTest.placedGameObjects = new();
        EndEvent();
    }



}
