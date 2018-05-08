using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingMeleeAbility : Ability
{
    public Collider[] damageColliders;

    public int minimumDamage;
    public int maximumDamage;

    int currentAttack;
    int lastAttack;

    protected override void Start()
    {
        base.Start();
        foreach(Collider damageCollider in damageColliders)
        damageCollider.GetComponent<DamageTrigger>().SetDamage(minimumDamage, maximumDamage);
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();

        print("Current Collider is " + currentAttack);
        damageColliders[currentAttack].enabled = true;

        lastAttack = currentAttack;
        if (currentAttack + 1 < numberOfAnimations)
            currentAttack++;
        else
            currentAttack = 0;

        StartCoroutine(DisableDamageCollider());
        TriggerCooldown();
    }

    public override void Animate()
    {
        if (playerAnimator)
        {
            playerAnimator.SpecificAttack(abilitySlot, currentAttack + 1);
        }
    }

    IEnumerator DisableDamageCollider()
    {
        yield return new WaitForSeconds(.1f);
        damageColliders[lastAttack].enabled = false;
    }
}
