using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] sounds;

    bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            if(!triggered)
            {
                triggered = true;
                AudioClip sound = sounds[Random.Range(0, sounds.Length)];
                source.PlayOneShot(sound);
            }
        }
    }
}
