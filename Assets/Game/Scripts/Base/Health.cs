using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int baseHealth;
    public float despawnTime;
    public int numberOfHits;
    public int numberOfDeaths;

    [HideInInspector]
    public bool dead;

    protected int health;
    protected Animator anim;

    public virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        ResetCharacter();
    }

    public virtual void TookDamage(int damage)
    {
        health -= damage;

        int randomHit = Random.Range(1, numberOfHits + 1);
        anim.SetInteger("Hit", randomHit);

        if (health <= 0 && !dead)
        {
            dead = true;
            StartCoroutine(Died());
        }
    }

    public virtual IEnumerator Died()
    {
        int randomDeath = Random.Range(1, numberOfDeaths + 1);
        anim.SetInteger("Dead", randomDeath);
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }

    public virtual void ResetCharacter()
    {
        health = baseHealth;
        anim.SetInteger("Dead", 0);
    }

    public void ResetHit()
    {
        anim.SetInteger("Hit", 0);
    }
}
