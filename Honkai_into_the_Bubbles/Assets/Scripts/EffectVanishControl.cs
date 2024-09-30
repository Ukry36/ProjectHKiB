using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectVanishControl : MonoBehaviour
{
    private enum VanishType { byAnim, byTime, byOrder }
    [SerializeField] private VanishType vanishType;
    [SerializeField] private Animator animator;
    [SerializeField] private float vanishTime = 5f;
    [SerializeField] private string SFX;
    private float timer = 0;


    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        AudioManager.instance.PlaySound(SFX, this.transform, _attachToTarget: false);
        timer = vanishTime;
    }
    private void Update()
    {
        if (vanishType == VanishType.byTime)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
                animator.SetTrigger("exit");
        }
    }

    public void AnimationFinishTrigger() => this.gameObject.SetActive(false);

    public void ExitbyOrder()
    {
        if (vanishType == VanishType.byOrder) animator.SetTrigger("exit");
    }
}
