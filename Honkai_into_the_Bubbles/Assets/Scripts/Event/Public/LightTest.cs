using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightTest : Event
{
    [SerializeField] private Light2D light2d;
    [SerializeField] private float step = 0.01f;
    [SerializeField] private TextMeshProUGUI text;

    protected override void StartEvent(Status _interactedEntity)
    {
        StartCoroutine(Cooltime());
        InputManager.instance.StopUIInput(triggerToExpire.activated);
        text.text = "Light Level = " + MathF.Round(light2d.intensity, 3) + "\nQ: darker\nE: brighter\nShift: step = " + step;
    }

    private void Update()
    {
        if (triggerToExpire.activated)
        {
            if (InputManager.instance.NextInput)
            {
                AudioManager.instance.PlaySound(initialSFX, this.transform);
                light2d.intensity += step;
                text.text = "Light Level = " + MathF.Round(light2d.intensity, 3) + "\nQ: darker\nE: brighter\nShift: step = " + step;
            }
            if (InputManager.instance.PrevInput)
            {
                AudioManager.instance.PlaySound(initialSFX, this.transform);
                light2d.intensity -= step;
                text.text = "Light Level = " + MathF.Round(light2d.intensity, 3) + "\nQ: darker\nE: brighter\nShift: step = " + step;
                light2d.intensity = light2d.intensity < 0 ? 0 : light2d.intensity;
            }
            if (InputManager.instance.ShiftInput)
            {
                AudioManager.instance.PlaySound(initialSFX, this.transform);
                if (step < 0.04f)
                    step = 0.05f;
                else if (step < 0.09f)
                    step = 0.1f;
                else if (step < 0.49f)
                    step = 0.5f;
                else if (step < 0.9f)
                    step = 0.01f;

                text.text = "Light Level = " + MathF.Round(light2d.intensity, 3) + "\nQ: darker\nE: brighter\nShift: step = " + step;
            }
        }
    }

    public override void EndEvent()
    {
        base.EndEvent();
    }


}
