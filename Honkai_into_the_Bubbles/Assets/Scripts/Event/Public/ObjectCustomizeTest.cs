using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCustomizeTest : Event
{

    [HideInInspector] public List<GameObject> placedGameObjects;

    [SerializeField] private ObjectPlacerTest objectPlacer;

    private bool inProgress;

    [SerializeField] private string endSFX = "trigger_toggle_off";


    protected override void StartEvent(Status _interactedEntity)
    {
        inProgress = true;
        StartCoroutine(Cooltime());
        objectPlacer.gameObject.SetActive(true);
        InputManager.instance.StopUIInput(true);
        for (int i = 0; i < placedGameObjects.Count; i++)
        {
            placedGameObjects[i].GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void Update()
    {
        if (inProgress && PlayerManager.instance.theStat.inTransferPosition) EndEvent();
        if (inProgress && InputManager.instance.CancelInput) EndEvent();

    }

    public override void EndEvent()
    {
        inProgress = false;
        objectPlacer.gameObject.SetActive(false);
        InputManager.instance.StopUIInput(false);
        for (int i = 0; i < placedGameObjects.Count; i++)
        {
            placedGameObjects[i].GetComponent<BoxCollider2D>().enabled = true;
        }
        AudioManager.instance.PlaySoundFlat(endSFX);
        triggerToExpire.activated = false;
        base.EndEvent();
    }


}
