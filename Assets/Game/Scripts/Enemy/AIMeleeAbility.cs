using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMeleeAbility : Ability
{
    PlayerHealth playerHealth;

    protected override void Start()
    {
        playerHealth = ReferenceManager.instance._player.GetComponent<PlayerHealth>();
    }

    public override void ActivateAbility()
    {
        int minDamage = abilityMinimumDamage;
        int maxDamage = abilityMaximumDamage;

        minDamage += (int)entityStats.GetStatCurrentValue(Stat.StatType.Damage);
        maxDamage += (int)entityStats.GetStatCurrentValue(Stat.StatType.Damage);

        int randomDamage = Random.Range(minDamage, maxDamage);

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

        playerHealth.TookDamage(randomDamage, gameObject, crit);
        TriggerCooldown();
    }
}
