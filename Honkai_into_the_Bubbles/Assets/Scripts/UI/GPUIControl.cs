using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GPUIControl : MonoBehaviour
{
    [SerializeField] private State theState;
    [SerializeField] private TextMeshPro TMPro;
    [SerializeField] private Transform guage;
    void Update()
    {
        guage.localScale = new Vector3(1, (float)theState.currentGP/theState.maxGP);
        TMPro.text = theState.currentGP.ToString();
    }
}
