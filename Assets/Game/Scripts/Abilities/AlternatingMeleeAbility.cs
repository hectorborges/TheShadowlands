using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingMeleeAbility : Ability
{
    public DamageTrigger[] damageTriggers;

    public int minimumDamage;
    public int maximumDamage;

    int currentAttack;
    int lastAttack;

    protected override void Start()
    {
        base.Start();
        foreach(DamageTrigger damageTrigger in damageTriggers)
            damageTrigger.SetDamage(minimumDamage, maximumDamage, this);
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();

        lastAttack = currentAttack;
        if (currentAttack + 1 < numberOfAnimations)
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
