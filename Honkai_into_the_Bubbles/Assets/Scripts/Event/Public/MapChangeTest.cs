using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public class Map
{
    public string name;
    public List<GameObject> gameObjects;
}
public class MapChangeTest : Event
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private List<Map> Maps;
    private int CurrentMap;
    protected override void StartEvent(Status _interactedEntity)
    {
        StartCoroutine(Cooltime());
        InputManager.instance.StopUIInput(triggerToExpire.activated);
        text.text = "Current Map = " + Maps[CurrentMap].name + "\nQ: prev\nE: next";
    }

    private void Start()
    {
        CurrentMap = 0;
        UpdateMap();
    }

    private void Update()
    {
        if (triggerToExpire.activated)
        {
            if (InputManager.instance.NextInput)
            {
                AudioManager.instance.PlaySound(initialSFX, this.transform);
                CurrentMap++;
                CurrentMap %= Maps.Count;
                text.text = "Current Map = " + Maps[CurrentMap].name + "\nQ: prev\nE: next";
                UpdateMap();
            }
            if (InputManager.instance.PrevInput)
            {
                AudioManager.instance.PlaySound(initialSFX, this.transform);
                CurrentMap--;
                if (CurrentMap < 0) CurrentMap += Maps.Count;
                text.text = "Current Map = " + Maps[CurrentMap].name + "\nQ: prev\nE: next";
                UpdateMap();
            }
        }
    }

    private void UpdateMap()
    {
        for (int i = 0; i < Maps.Count; i++)
        {
            if (i != CurrentMap)
            {
                for (int j = 0; j < Maps[i].gameObjects.Count; j++)
                {
                    Maps[i].gameObjects[j].SetActive(false);
                }
            }
        }

        for (int j = 0; j < Maps[CurrentMap].gameObjects.Count; j++)
        {
            Maps[CurrentMap].gameObjects[j].SetActive(true);
        }
    }

    public override void EndEvent()
    {
        base.EndEvent();
    }


}
