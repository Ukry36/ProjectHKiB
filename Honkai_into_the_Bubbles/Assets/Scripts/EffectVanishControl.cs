using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectVanishControl : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool byTime;
    [SerializeField] private float vanishTime = 5f;
    [SerializeField] private bool byOrder;
    [SerializeField] private string SFX;
    private float time = 0;

    private void OnEnable()
    {
        AudioManager.instance.PlaySound(SFX, this.transform, _attachToTarget: false);
    }
    private void Update()
    {
        if (!byOrder)
        {
            if (byTime)
            {
                time += Time.deltaTime;
                if (time >= vanishTime)
                    this.gameObject.SetActive(false);
            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
                    this.gameObject.SetActive(false);
            }
        }


    }

    public void Exit()
    {
        animator.SetTrigger("exit");
        StartCoroutine(ExitCoroutine());
    }

    private IEnumerator ExitCoroutine()
    {
        yield return null;
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f);
        this.gameObject.SetActive(false);
    }
}
