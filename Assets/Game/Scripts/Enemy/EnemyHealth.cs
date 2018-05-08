using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    public override void TookDamage(int damage)
    {
        health -= damage;

        int randomHit = Random.Range(1, numberOfHits + 1);
        anim.SetInteger("Hit", randomHit);

        if(health <= 0 && !dead)
        {
            dead = true;
            StartCoroutine(Died());
        }
    }
}
