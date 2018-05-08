using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public delegate void AbilityEvents(Ability _thisAbility);
    public event AbilityEvents OnCooldownFinished;

    public string abilityName;
    public Sprite abilityIcon;
    [TextArea]
    public string abilityDescription;

    public enum AbilitySlot
    {
        PrimaryAbility,
        SecondaryAbility,
        AbilityOne,
        AbilityTwo,
        AbilityThree,
        AbilityFour
    };

    public AbilitySlot abilitySlot;

    public enum AbilityInput
    {
        GetButtonDown,
        GetButton,
        GetButtonUp
    };

    public AbilityInput abilityInput;

    [Range(1, 5)]
    public int abilityCharges = 1;
    public float chargeTime;
    public float abilityCooldown;

    [Space, Header("Extra Options")]
    public PlayerAnimator playerAnimator;
    public int numberOfAnimations;

    [HideInInspector]
    public int charges;
    bool onCooldown;
    [HideInInspector] public bool isCharging;
    [HideInInspector] public bool isFiring;

    Coroutine charge;

    protected virtual void Start()
    {
        charges = abilityCharges;
    }

    public virtual void ActivateAbility()
    {
        print(abilityName + " Activated!");
        if (!isCharging)
        {
            charge = StartCoroutine(Charge());
            charges--;
        }
        Animate();

    }

    public virtual void Animate()
    {
        if (playerAnimator)
        {
            playerAnimator.Attack(abilitySlot, numberOfAnimations);
        }
    }

    public virtual void DeactivateAbility()
    {
        if (charge != null)
        {
            StopCoroutine(charge);
            isCharging = false;
        }
    }

    public virtual void VisualOnCharge() { }
    public virtual void VisualOnActivate() { }
    public virtual void VisualOnDeactivate() { }

    //This should be called in the custom ability script
    public void TriggerCooldown()
    {
        onCooldown = true;
        StartCoroutine(Cooldown());
    }

    public bool CanShoot()
    {
        if (charges > 0)
            return true;
        else return false;
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(abilityCooldown);

        if (charges < abilityCharges)
            charges++;

        if (OnCooldownFinished != null)
            OnCooldownFinished(this);

        onCooldown = false;
    }

    protected virtual IEnumerator Charge()
    {
        isCharging = true;
        yield return new WaitForSeconds(chargeTime);
        isCharging = false;
    }
}
