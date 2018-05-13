using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownReductionAbility : Ability
{
    [Space]
    public Ability targetAbility;
    public float newCooldown;
    public float cooldownReductionLength;

    float baseCooldown;

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        TriggerCooldown();
        baseCooldown = targetAbility.abilityCooldown;

        targetAbility.abilityCooldown = newCooldown;
        targetAbility.abilityInput = AbilityInput.GetButton;
        StartCoroutine(CooldownReductionLength());
    }

    IEnumerator CooldownReductionLength()
    {
        yield return new WaitForSeconds(cooldownReductionLength);
        targetAbility.abilityCooldown = baseCooldown;
        targetAbility.abilityInput = AbilityInput.GetButtonDown;
    }
}
