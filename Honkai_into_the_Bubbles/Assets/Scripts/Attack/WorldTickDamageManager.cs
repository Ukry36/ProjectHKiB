using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTickDamageManager : MonoBehaviour
{
    public TickDamage DamageInfo;

    private void Start()
    {
        DamageInfo.isCooltime = false;
    }

    private void Update()
    {
        if (!DamageInfo.isCooltime)
        {
            StartCoroutine(PlayerManager.instance.theStat.TickDamageCoroutine(DamageInfo));
            StartCoroutine(TickDamageCooltimeCoroutine());
        }
    }

    private IEnumerator TickDamageCooltimeCoroutine()
    {
        DamageInfo.isCooltime = true;
        yield return new WaitForSeconds(DamageInfo.Cooltime);
        DamageInfo.isCooltime = false;
    }
}
