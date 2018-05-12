using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorBase : MonoBehaviour
{
    public Animator animator;

    public void Move(bool state)
    {
        animator.SetBool("IsMoving", state);
    }

    public void Move(float speed, float baseSpeed)
    {
        animator.SetFloat("Speed", speed / baseSpeed);
    }

    public virtual void Dead(bool state)
    {
        animator.SetBool("IsDead", state);
    }

    public void Attack(int numerOfAttackAnimations)
    {
        int randomAttack = Random.Range(1, numerOfAttackAnimations + 1);
        animator.SetInteger("Attack", randomAttack);
    }

    public void SpecificAttack(int attackToTrigger)
    {
        animator.SetInteger("Attack", attackToTrigger);
    }

    public void Hit(int numberOfHitAnimations)
    {
        int randomHit = Random.Range(1, numberOfHitAnimations + 1);
        animator.SetInteger("Hit", randomHit);
    }

    public void Death(int numberOfDeathAnimations)
    {
        int randomDeath = Random.Range(1, numberOfDeathAnimations + 1);
        animator.SetInteger("Death", randomDeath);
    }

    public virtual void ResetAttack(ResetAttack.AbilitySlot abilitySlot)
    {

    }

    public void ResetAttack()
    {
        animator.SetInteger("Attack", 0);
    }

    public void ResetHit()
    {
        animator.SetInteger("Hit", 0);
    }

    public void ResetDeath()
    {
        animator.SetInteger("Death", 0);
    }
}
