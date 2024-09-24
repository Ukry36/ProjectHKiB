using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GPUIControl : MonoBehaviour
{
    [SerializeField] private Status theStat;
    [SerializeField] private TextMeshPro TMPro;
    [SerializeField] private Transform guage;
    void Update()
    {
        guage.localScale = new Vector3(1, (float)theStat.CurrentGP / theStat.currentMaxGP);
        TMPro.text = theStat.CurrentGP.ToString();
    }
}
