using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnSetActive : MonoBehaviour
{
    public AudioClip onEnableSound;
    public AudioClip onDisableSound;
    public AudioSource source;

    private void OnEnable()
    {
        source.PlayOneShot(onEnableSound);
    }

    private void OnDisable()
    {
        source.PlayOneShot(onDisableSound);
    }
}
