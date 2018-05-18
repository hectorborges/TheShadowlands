using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffects : MonoBehaviour
{
    [Header("References"), Space]
    public Health health;
    public AI ai;

    [Header("Status Effects"), Space]
    public GameObject bleedEffect;
    public GameObject burnEffect;
    public GameObject stunEffect;
    public GameObject slowEffect;

    Coroutine bleed;
    Coroutine burn;
    Coroutine stun;
    Coroutine slow;

    bool bleeding;
    bool burning;
    bool stunned;
    bool slowed;

    public void Bleed(float length, int damage)
    {
        if (bleed != null)
            StopCoroutine(bleed);

        bleed = StartCoroutine(Bleeding(length, damage));
    }

    public void Burn(float length, int damage)
    {
        if (burn != null)
            StopCoroutine(burn);

        burn = StartCoroutine(Burning(length, damage));
    }

    public void Stun(float length)
    {
        if (stun != null)
            StopCoroutine(stun);

        stun = StartCoroutine(Stunned(length));
    }

    public void Slow(float length)
    {
        if (slow != null)
        {
            ai.ResetSpeed();
            StopCoroutine(slow);
        }

        slow = StartCoroutine(Slowed(length));
    }

    IEnumerator Bleeding(float length, int damage)
    {
        for(int i = 0; i < length; i++)
        {
            bleedEffect.SetActive(true);
            health.TookDamage(damage);
            yield return new WaitForSeconds(1);
        }
        bleeding = false;
    }

    IEnumerator Burning(float length, int damage)
    {
        burnEffect.SetActive(true);
        for (int i = 0; i < length; i++)
        {
            health.TookDamage(damage);
            yield return new WaitForSeconds(1);
        }
        burnEffect.SetActive(false);
        burning = false;
    }

    IEnumerator Stunned(float length)
    {
        ai.SetStunned(true);
        stunEffect.SetActive(true);
        yield return new WaitForSeconds(length);
        stunEffect.SetActive(false);
        ai.SetStunned(false);
        stunned = false;
    }

    IEnumerator Slowed(float length)
    {
        ai.SetSlowed(true);
        float slowAmount = ai.agent.speed * .5f;
        ai.SetSpeed(slowAmount);
        slowEffect.SetActive(true);
        yield return new WaitForSeconds(length);
        slowEffect.SetActive(false);
        ai.ResetSpeed();
        ai.SetSlowed(false);
        slowed = false;
    }
}
