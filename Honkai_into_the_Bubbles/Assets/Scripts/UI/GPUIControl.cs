using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GPUIControl : MonoBehaviour
{
    [SerializeField] private Status theState;
    [SerializeField] private TextMeshPro TMPro;
    [SerializeField] private Transform guage;
    void Update()
    {
        guage.localScale = new Vector3(1, (float)theState.CurrentGP / theState.maxGP);
        TMPro.text = theState.CurrentGP.ToString();
    }
}
