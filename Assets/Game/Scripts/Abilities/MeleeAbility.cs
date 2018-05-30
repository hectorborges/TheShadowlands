using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbility : Ability
{
    public DamageTrigger damageTrigger;

    protected override void Start()
    {
        base.Start();
        damageTrigger.SetDamage(abilityMinimumDamage, abilityMaximumDamage, this);
    }

    public override void ActivateAbility()
    {
        if (!CanShoot()) return;
        base.ActivateAbility();
        
        TriggerCooldown();
    }
}
