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

    public void ResetAttack(ResetAttack.AbilitySlot abilitySlot)
    {
        animator.SetInteger(abilitySlot.ToString(), 0);
    }

    public void ResetHit()
    {
        animator.SetInteger("Hit", 0);
    }
}
