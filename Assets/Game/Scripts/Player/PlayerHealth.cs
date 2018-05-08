using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    protected PlayerAnimator playerAnimator;

    public void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        ResetCharacter();
    }

    public override void TookDamage(int damage)
    {
        playerAnimator.Hit(numberOfHits);
        base.TookDamage(damage);
    }

    public override IEnumerator Died()
    {
        PlayerMovement.canMove = false;
        AudioClip deathSound = deathSounds[Random.Range(0, deathSounds.Length)];
        source.PlayOneShot(deathSound);

        playerAnimator.Dead(true);
        yield return new WaitForSeconds(despawnTime);
    }

    public virtual void ResetCharacter()
    {
        health = baseHealth;
        playerAnimator.Dead(false);
    }

    public void Ressurected()
    {
        PlayerMovement.canMove = true;
    }
}