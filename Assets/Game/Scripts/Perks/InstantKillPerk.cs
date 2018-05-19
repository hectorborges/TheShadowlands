using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantKillPerk : Perk
{
    [Header("Required Variables"), Space, Range(0, 1)]
    public float healthPercentage;

    public override void Procced(StatusEffects statusEffects)
    {
        statusEffects.Execute(healthPercentage);
    }
}
