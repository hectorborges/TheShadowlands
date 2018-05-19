using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTookDamagePerk : Perk
{
    [Header("Required References"), Space]
    public float damagePercentage;

    public override void Procced()
    {
        PlayerHealth.instance.thornsDamagePercentage = damagePercentage;
        PlayerHealth.instance.SetThornsActive(true);
    }
}
