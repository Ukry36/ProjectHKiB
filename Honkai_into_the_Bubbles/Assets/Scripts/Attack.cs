using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{
    public int DamageCoefficient; // Coefficient of atk
    public int TrackingRadius; // teleport in front of enemy on tracking area
    public int CriticalRate; // possibility of critical
    public int GraffitiPoint; // amount of GraffitiPoint to obtain
    public bool Strong; // cause hit motion

    public Attack(int _DmgCoeff, int _TrackRad = 0, int _CritRate = 10, bool _Strong = false, int _GP = 1)
    {
        DamageCoefficient = _DmgCoeff;
        TrackingRadius = _TrackRad;
        CriticalRate = _CritRate;
        GraffitiPoint = _GP;
        Strong = _Strong;
    }
}
