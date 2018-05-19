using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmunePerk : Perk
{
    [Header("Required References"), Space]
    public PlayerHealth playerHealth;
    public float immuneChance;

    public override void Procced()
    {
        playerHealth.SetImmunity(immuneChance);
    }
}
