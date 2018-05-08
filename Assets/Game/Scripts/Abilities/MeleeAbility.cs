using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbility : Ability
{
    public Collider damageCollider;
    public int minimumDamage;
    public int maximumDamage;

    protected override void Start()
    {
        base.Start();
        damageCollider.GetComponent<DamageTrigger>().SetDamage(minimumDamage, maximumDamage);
    }

    public override void ActivateAbility()
    {
        if (!CanShoot()) return;
        base.ActivateAbility();
        
        damageCollider.enabled = true;
        StartCoroutine(DisableDamageCollider());
        TriggerCooldown();
    }

    IEnumerator DisableDamageCollider()
    {
        yield return new WaitForSeconds(.2f);
        damageCollider.enabled = false;
    }
}
