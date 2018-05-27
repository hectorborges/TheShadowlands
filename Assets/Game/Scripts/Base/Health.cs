﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Stats stats;
    public int baseHealth;
    public float despawnTime;
    public int numberOfHits;
    public int numberOfDeaths;

    public AudioSource source;
    public AudioClip[] hitSounds;
    public AudioClip[] deathSounds;

    [HideInInspector] public bool isDead;
    public int health;

    float immuneChance;

    public virtual void TookDamage(int damage, GameObject attackingTarget, bool crit)
    {
        if (Immunity(immuneChance)) return;
        health -= damage;

        if(hitSounds.Length > 0)
        {
            AudioClip hitSound = hitSounds[Random.Range(0, hitSounds.Length)];
            source.PlayOneShot(hitSound);
        }

        if (health <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(Died());
        }
    }

    public virtual void TookDamage(int damage)
    {
        health -= damage;

        if(hitSounds.Length > 0)
        {
            AudioClip hitSound = hitSounds[Random.Range(0, hitSounds.Length)];
            source.PlayOneShot(hitSound);
        }

        if (health <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(Died());
        }
    }

    public void SetImmunity(float _immuneChance)
    {
        immuneChance = _immuneChance;
    }

    bool Immunity(float immuneChance)
    {
        float random = Random.Range(0, 100);

        if (immuneChance <= random && immuneChance != 0)
        {
            print("Immune");
            return true;
        }
        else return false;
    }

    public virtual IEnumerator Died()
    {
        AudioClip deathSound = deathSounds[Random.Range(0, deathSounds.Length)];
        source.PlayOneShot(deathSound);

        yield return new WaitForSeconds(despawnTime);
        //Destroy(gameObject);
    }

    public virtual void GainHealth(int healthGained)
    {
        health += healthGained;
    }

    public virtual void UpdateBaseHealth()
    {
        baseHealth = (int)stats.GetStatBaseValue(Stat.StatType.Health);
    }

    public virtual void HealthGainOnHit()
    {
        int healthGained = (int)stats.GetStatCurrentValue(Stat.StatType.HealthPerHit);

        if (health + healthGained <= baseHealth)
            health += healthGained;
        else
            health = baseHealth;
    }
}
