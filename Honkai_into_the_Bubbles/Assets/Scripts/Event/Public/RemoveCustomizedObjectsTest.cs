using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveCustomizedObjectsTest : Event
{

    [SerializeField] private ObjectCustomizeTest OCTest;

    protected override void StartEvent(Status _interactedEntity)
    {
        List<GameObject> gameObjects = OCTest.placedGameObjects;
        for (int i = 0; i < gameObjects.Count; i++)
        {
            Destroy(gameObjects[i]);
        }
        OCTest.placedGameObjects = new();
        EndEvent();
    }



}
