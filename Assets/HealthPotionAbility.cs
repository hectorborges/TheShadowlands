using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPotionAbility : Ability
{
    public delegate void AbilityEvents(Item _item);
    public event AbilityEvents OnCooldownFinished;

    [Range(.1f, .99f)]
    public float healPercentage = .4f;
    public Text abilityCooldownText;

    protected override void Start()
    {
        base.Start();
    }
    public override void ActivateAbility()
    {
        base.ActivateAbility();
        int healAmount = Mathf.RoundToInt(entityHealth.baseHealth * healPercentage);
        print("Healing for: " + healAmount);
        entityHealth.GainHealth(healAmount);
        TriggerCooldown();
    }

    protected override IEnumerator Cooldown()
    {
        for(int i = (int)abilityCooldown; i > 0; i--)
        {
            abilityCooldownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        abilityCooldownText.text = "";
        if (currentCharges < abilityCharges)
            currentCharges++;

        if (OnCooldownFinished != null)
            OnCooldownFinished(abilityItem);

        onCooldown = false;
    }
}
