using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Friendly : Entity
{
    protected Transform Player;
    [BoxGroup("Move Algorythm")]
    public LayerMask targetLayer;
    [BoxGroup("Move Algorythm")]
    public float endFollowRadius = 12;
    [BoxGroup("Move Algorythm")]
    public float followRadius = 8;

    protected override void Awake()
    {
        base.Awake();
        Player = PlayerManager.instance.transform;

    }

}
