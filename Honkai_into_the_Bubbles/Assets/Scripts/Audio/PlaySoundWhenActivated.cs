using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundWhenActivated : MonoBehaviour
{
    [SerializeField] private string sound;
    private void OnEnable()
    {
        AudioManager.instance.PlaySound(sound, this.transform);
    }
}
