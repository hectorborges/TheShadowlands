using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int baseHealth;
    public float despawnTime;
    public int numberOfHits;
    public int numberOfDeaths;

    public AudioSource source;
    public AudioClip[] hitSounds;
    public AudioClip[] deathSounds;

    [HideInInspector]
    public bool dead;

    protected int health;

    public virtual void TookDamage(int damage)
    {
        health -= damage;

        AudioClip hitSound = hitSounds[Random.Range(0, hitSounds.Length)];
        source.PlayOneShot(hitSound);

        if (health <= 0 && !dead)
        {
            dead = true;
            StartCoroutine(Died());
        }
    }

    public virtual IEnumerator Died()
    {
        AudioClip deathSound = deathSounds[Random.Range(0, deathSounds.Length)];
        source.PlayOneShot(deathSound);

        yield return new WaitForSeconds(despawnTime);
        //Destroy(gameObject);
    }
}
