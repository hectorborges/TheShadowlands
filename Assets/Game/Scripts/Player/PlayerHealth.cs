using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    protected PlayerAnimator playerAnimator;
    public static PlayerHealth instance;
    public Perk[] onDamagedPerks;

    [HideInInspector]public float thornsDamagePercentage;
    bool thorns;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        ResetCharacter();
    }

    public override void TookDamage(int damage, GameObject attackingTarget)
    {
        playerAnimator.Hit(numberOfHits);
        base.TookDamage(damage, attackingTarget);

        if(thorns)
        {
            float thornsDamage = baseHealth / thornsDamagePercentage;
            attackingTarget.GetComponent<Health>().TookDamage((int)thornsDamage, gameObject);
        }
    }

    public void SetThornsActive(bool status)
    {
        thorns = status;
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