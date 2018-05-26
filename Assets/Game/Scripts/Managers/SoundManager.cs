using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> needManaSounds;
    public AudioSource needManaSource;

    public void NeedMana()
    {
        if(!needManaSource.isPlaying)
        {
            needManaSource.clip = needManaSounds[Random.Range(0, needManaSounds.Count)];
            needManaSource.Play();
        }
    }
}
