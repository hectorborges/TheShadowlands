using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

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

    public enum AbilityType
    {
        Axe,
        Shield,
        Swords,
        Rifle,
        Pistols,
        Magic
    };

    public AbilityType abilityType;

    public AbilityInput abilityInput;

    public Stats stats;
    public Health entityHealth;
    public Mana mana;

    [Range(1, 5)]
    public int abilityCharges = 1;
    public float chargeTime;
    public float abilityCooldown;
    public float attackDistance;
    public int manaGained;
    public int manaRequired;

    [Space, Header("Perks")]
    public Perk[] procOnAttackPerks;
    public Perk[] procOnKillsPerks;

    [Space, Header("Extra Options")]
    public PlayerAnimator playerAnimator;
    public int numberOfAnimations;

    [HideInInspector]
    public int charges;
    bool onCooldown;
    [HideInInspector] public bool isCharging;
    [HideInInspector] public bool isFiring;

    public bool requiresTarget;

    [Space, Header("Camera Shake")]
    public float magnitude = 4f;
    public float roughness = 4f;
    public float fadeIn = .1f;
    public float fadeOut = 1f;

    bool shouldHeal;

    Coroutine charge;
    Coroutine cooldown;

    protected virtual void Start()
    {
        charges = abilityCharges;
    }

    public virtual void ActivateAbility()
    {
        if (entityHealth.isDead) return;

        if(mana)
        {
            if (!mana.ActivateAbility(manaRequired))
                return;
            mana.GainMana(manaGained);
        }

        shouldHeal = true;
        PlayerMovement.canMove = false;
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
        PlayerMovement.canMove = true;
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
        if (entityHealth.isDead) return;
        onCooldown = true;
        cooldown = StartCoroutine(Cooldown());
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

    public bool CheckCooldown()
    {
        return onCooldown;
    }

    public void ResetCooldown()
    {
        if(cooldown != null)
        {
            StopCoroutine(cooldown);

            if (charges < abilityCharges)
                charges++;

            if (OnCooldownFinished != null)
                OnCooldownFinished(this);

            onCooldown = false;
            PlayerLoadout.instance.ResetCooldown(this);
        }
    }

    protected virtual IEnumerator Charge()
    {
        isCharging = true;
        yield return new WaitForSeconds(chargeTime);
        isCharging = false;
    }

    public void DealDamage(int minimumDamage, int maximumDamage, GameObject other)
    {
        Health health = other.GetComponent<Health>();

        minimumDamage += (int)stats.GetStatCurrentValue(Stat.StatType.Damage);
        maximumDamage += (int)stats.GetStatCurrentValue(Stat.StatType.Damage);

        int randomDamage = Random.Range(minimumDamage, maximumDamage);

        int critRoll = Random.Range(0, 100);

        bool crit;
        if ((int)stats.GetStatCurrentValue(Stat.StatType.CriticalStrike) <= critRoll)
        {
            crit = true;
            float newDamage = randomDamage;
            newDamage *= stats.GetStatCurrentValue(Stat.StatType.CriticalDamage);
            randomDamage = (int)newDamage;
        }
        else
            crit = false;

        health.TookDamage(randomDamage, gameObject, crit);


        if(shouldHeal)
        {
            shouldHeal = false;
            CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeIn, fadeOut);
            entityHealth.GainHealth((int)stats.GetStatCurrentValue(Stat.StatType.HealthPerHit));
        }

        for (int i = 0; i < procOnAttackPerks.Length; i++)
        {
            if (procOnAttackPerks[i].activated)
            {
                if (procOnAttackPerks[i].requiresStatusEffects)
                {
                    procOnAttackPerks[i].ActivatePerk(other.GetComponent<StatusEffects>());
                }
                else
                    procOnAttackPerks[i].ActivatePerk();
            }
        }

        if(health.isDead)
        {
            for (int i = 0; i < procOnKillsPerks.Length; i++)
            {
                if (procOnKillsPerks[i].activated)
                {
                    procOnKillsPerks[i].ActivatePerk();
                }
            }
        }
    }
}
