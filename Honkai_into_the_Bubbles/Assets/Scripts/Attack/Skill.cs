using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : Attack
{
    public float DetectRadius; // detect radius to activate Tracking
    public float Cooltime; // cooldown time 
    public float Duration; // skill duration
    public float Delay; // delay before or between atk 
    public float animationPlayTime; // must be larger than Delay 
    [HideInInspector] public List<Vector2> skillCommand;
    [HideInInspector] public bool isCooltime;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

    }
}