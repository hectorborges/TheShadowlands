using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : AnimatorBase
{
    public AnimatorOverrideController twoHandedSwordAOC;

    public void Attack(Ability.AbilitySlot abilitySlot, int numerOfAttackAnimations)
    {
        int randomAttack = Random.Range(1, numerOfAttackAnimations + 1);
        animator.SetInteger(abilitySlot.ToString(), randomAttack);
    }

    public void SpecificAttack(Ability.AbilitySlot abilitySlot, int attackToTrigger)
    {
        animator.SetInteger(abilitySlot.ToString(), attackToTrigger);
    }

    public override void ResetAttack(ResetAttack.AbilitySlot abilitySlot)
    {
        animator.SetInteger(abilitySlot.ToString(), 0);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            OverrideAnimations();
    }

    public void OverrideAnimations()
    {
        animator.runtimeAnimatorController = twoHandedSwordAOC;
    }
}
