using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;

    public void Move(bool state)
    {
        animator.SetBool("IsMoving", state);
    }

    public void Dead(bool state)
    {
        animator.SetBool("IsDead", state);
    }

    public void Attack(Ability.AbilitySlot abilitySlot, int numerOfAttackAnimations)
    {
        int randomAttack = Random.Range(1, numerOfAttackAnimations + 1);
        animator.SetInteger(abilitySlot.ToString(), randomAttack);
    }

    public void SpecificAttack(Ability.AbilitySlot abilitySlot, int attackToTrigger)
    {
        animator.SetInteger(abilitySlot.ToString(), attackToTrigger);
    }

    public void Hit(int numberOfHitAnimations)
    {
        int randomHit = Random.Range(1, numberOfHitAnimations + 1);
        animator.SetInteger("Hit", randomHit);
    }

    public void ResetAttack(ResetAttack.AbilitySlot abilitySlot)
    {
        animator.SetInteger(abilitySlot.ToString(), 0);
    }

    public void ResetHit()
    {
        animator.SetInteger("Hit", 0);
    }
}
