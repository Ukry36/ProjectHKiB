using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public class Difficulty
{
    public string name;
    public WaveSequence waveSequence;
}

public class DifficultyChange : Event
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private List<Difficulty> Difficulties;
    private int CurrentDiff;

    [SerializeField] private WaveManager2 waveManager2;
    protected override void StartEvent(Status _interactedEntity)
    {
        StartCoroutine(Cooltime());
        InputManager.instance.StopUIInput(triggerToExpire.activated);
        text.text = "Difficulty\n" + Difficulties[CurrentDiff].name + "\nQ: -\nE: +";
    }

    private void Start()
    {
        CurrentDiff = 0;
        UpdateDiff();
    }

    private void Update()
    {
        if (triggerToExpire.activated)
        {
            if (InputManager.instance.NextInput)
            {
                AudioManager.instance.PlaySound(initialSFX, this.transform);
                CurrentDiff++;
                if (CurrentDiff >= Difficulties.Count) CurrentDiff--;
                text.text = "Difficulty\n" + Difficulties[CurrentDiff].name + "\nQ: -\nE: +";
                UpdateDiff();
            }
            if (InputManager.instance.PrevInput)
            {
                AudioManager.instance.PlaySound(initialSFX, this.transform);
                CurrentDiff--;
                if (CurrentDiff < 0) CurrentDiff++;
                text.text = "Difficulty\n" + Difficulties[CurrentDiff].name + "\nQ: -\nE: +";
                UpdateDiff();
            }
        }
    }

    private void UpdateDiff()
    {
        waveManager2.WaveSequence = Difficulties[CurrentDiff].waveSequence;
    }

    public override void EndEvent()
    {
        base.EndEvent();
    }


}
