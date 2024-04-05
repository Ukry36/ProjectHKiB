using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public int DamageCoefficient; // Coefficient of atk
    public int TrackingRadius; // teleport in front of enemy on tracking area
    public float DetectRadius; // detect radius to activate Tracking
    public int CriticalRate; // possibility of critical
    public bool Strong; // cause hit motion
    public float Cooltime; // cooldown time
    public bool CanSkill; // is cooltime ended?
    public float Delay;
    public List<Vector2> skillCommand;

    public int GP = 0;

    public Skill(int _DmgCoeff, int _TrackRad = 0, float _DetectRad = 1f, float _Cooldown = 0, float _Delay = 0, bool _Strong = false, int _CritRate = 10, bool _Can = true)
    {
        DamageCoefficient = _DmgCoeff;
        TrackingRadius = _TrackRad;
        DetectRadius = _DetectRad;
        CriticalRate = _CritRate;
        Strong = _Strong;
        Cooltime = _Cooldown;
        CanSkill = _Can;
        Delay = _Delay;
    }
}
