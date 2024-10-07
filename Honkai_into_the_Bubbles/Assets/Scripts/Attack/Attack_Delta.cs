using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Attack_Delta : Attack
{
    [SerializeField] private Delta_Delta delta;
    [SerializeField][MaxValue(100)][MinValue(0)] private int attackGuagefillAmount;

    protected override void OnEnable()
    {
        base.OnEnable();
        delta.AttackGuageManage(attackGuagefillAmount);
    }

}
