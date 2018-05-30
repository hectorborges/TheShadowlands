using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingMeleeAbility : Ability
{
    public DamageTrigger[] damageTriggers;

    int currentAttack;
    int lastAttack;

    protected override void Start()
    {
        base.Start();
        foreach(DamageTrigger damageTrigger in damageTriggers)
            damageTrigger.SetDamage(abilityMinimumDamage, abilityMaximumDamage, this);
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();

        lastAttack = currentAttack;
        if (currentAttack + 1 < abilityNumberOfAnimations)
            currentAttack++;
        else
            currentAttack = 0;
        
        TriggerCooldown();
    }

    public override void Animate()
    {
        if (playerAnimator)
        {
            playerAnimator.SpecificAttack(abilitySlot, currentAttack + 1);
        }
    }
}
