using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbility : Ability
{
    public DamageTrigger damageTrigger;
    public int minimumDamage;
    public int maximumDamage;

    protected override void Start()
    {
        base.Start();
        damageTrigger.SetDamage(minimumDamage, maximumDamage);
    }

    public override void ActivateAbility()
    {
        if (!CanShoot()) return;
        base.ActivateAbility();
        
        TriggerCooldown();
    }
}
