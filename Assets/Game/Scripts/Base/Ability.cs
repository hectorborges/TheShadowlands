using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public delegate void AbilityEvents(Item _item);
    public event AbilityEvents OnCooldownFinished;

    [Space, Header("Basic Information")]
    public string abilityName;
    public Sprite abilityIcon;
    [TextArea]
    public string abilityDescription;

    public enum MinimumRarity { Common, Rare, Epic, Legendary, Exotic, Artifact };
    public MinimumRarity minimumRarity;

    public enum AbilityInput { GetButtonDown, GetButton, GetButtonUp };
    public AbilityInput abilityInput;

    [Space, Header("Custom Variables")]
    public int abilityMinimumDamage;
    public int abilityMaximumDamage;
    public float abilityRange;
    public float abilityCastTime;
    public float abilityCooldown;
    public int abilityCharges;
    public int manaGained;
    public int manaRequired;

    public bool requiresTarget;

    [Space, Header("Required Variables")]
    public Stats entityStats;                                                                       //This is the character's actual stats
    public Health entityHealth;
    public Mana entityMana;

    [Space]
    public PlayerAnimator playerAnimator;
    public int abilityNumberOfAnimations;

    [Space, Header("Optional Variables")]
    public Weapon abilityWeapon;

    [Space, Header("Camera Shake")]
    public float magnitude = 4f;
    public float roughness = 4f;
    public float fadeIn = .1f;
    public float fadeOut = 1f;

    [HideInInspector] public int currentCharges;
    [HideInInspector] public bool isCasting;
    [HideInInspector] public bool isActivated;

    List<Stat> abilityStats = new List<Stat>();
    List<Perk> abilityPerks = new List<Perk>();

    bool onCooldown;
    bool shouldHeal;

    Coroutine cast;
    Coroutine cooldown;

    public enum AbilitySlot { PrimaryAbility, SecondaryAbility, AbilityOne, AbilityTwo, AbilityThree, AbilityFour };
    AbilitySlot abilitySlot;

    protected virtual void Start()
    {
        currentCharges = abilityCharges;
    }

    #region Activation
    public virtual void ActivateAbility()
    {
        if (entityHealth.isDead) return;                                                       

        if(entityMana)                                                                                      
        {
            if (!entityMana.ActivateAbility(manaRequired))
                return;
            entityMana.GainMana(manaGained);
        }

        shouldHeal = true;
        PlayerMovement.canMove = false;
        if (!isCasting)
        {
            cast = StartCoroutine(Casting());
            currentCharges--;
        }
        Animate();
    }

    public virtual void DeactivateAbility()
    {
        PlayerMovement.canMove = true;
        if (cast != null)
        {
            StopCoroutine(cast);
            isCasting = false;
        }
    }

    public virtual void VisualOnCharge() { }
    public virtual void VisualOnActivate() { }
    public virtual void VisualOnDeactivate() { }

    #endregion

    #region Animation
    public virtual void Animate()
    {
        if (playerAnimator)
        {
            playerAnimator.Attack(abilitySlot, abilityNumberOfAnimations);
        }
    }
    #endregion

    #region Functionality
    public void TriggerCooldown()
    {
        if (entityHealth.isDead) return;
        onCooldown = true;
        cooldown = StartCoroutine(Cooldown());
    }

    public bool CanShoot()
    {
        if (currentCharges > 0)
            return true;
        else return false;
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(abilityCooldown);

        if (currentCharges < abilityCharges)
            currentCharges++;

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

            if (currentCharges < abilityCharges)
                currentCharges++;

            if (OnCooldownFinished != null)
                OnCooldownFinished(this);

            onCooldown = false;
            PlayerLoadout.instance.ResetCooldown(this);
        }
    }

    protected virtual IEnumerator Casting()
    {
        isCasting = true;
        yield return new WaitForSeconds(abilityCastTime);
        isCasting = false;
    }
    #endregion

    #region Damage

    public void DealDamage(int minimumDamage, int maximumDamage, GameObject other)
    {
        Health health = other.GetComponent<Health>();

        minimumDamage += (int)entityStats.GetStatCurrentValue(Stat.StatType.Damage);
        maximumDamage += (int)entityStats.GetStatCurrentValue(Stat.StatType.Damage);

        int randomDamage = Random.Range(minimumDamage, maximumDamage);

        int critRoll = Random.Range(0, 100);

        bool crit;
        if ((int)entityStats.GetStatCurrentValue(Stat.StatType.CriticalStrike) <= critRoll)
        {
            crit = true;
            float newDamage = randomDamage;
            newDamage *= entityStats.GetStatCurrentValue(Stat.StatType.CriticalDamage);
            randomDamage = (int)newDamage;
        }
        else
            crit = false;

        health.TookDamage(randomDamage, gameObject, crit);


        if(shouldHeal)
        {
            shouldHeal = false;
            CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeIn, fadeOut);
            entityHealth.GainHealth((int)entityStats.GetStatCurrentValue(Stat.StatType.HealthPerHit));
        }

        for (int i = 0; i < abilityPerks.Count; i++)
        {
            if(abilityPerks[i].perkType == Perk.PerkType.ProcOnAttack)
            {
                if (abilityPerks[i].activated)
                {
                    if (abilityPerks[i].requiresStatusEffects)
                    {
                        abilityPerks[i].ActivatePerk(other.GetComponent<StatusEffects>());
                    }
                    else
                        abilityPerks[i].ActivatePerk();
                }
            }
        }

        if(health.isDead)
        {
            for (int i = 0; i < abilityPerks.Count; i++)
            {
                if (abilityPerks[i].perkType == Perk.PerkType.ProcOnKill)
                {
                    if (abilityPerks[i].activated)
                    {
                        abilityPerks[i].ActivatePerk();
                    }
                }
            }
        }
    }
    #endregion
}
