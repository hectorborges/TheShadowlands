using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCooldownPerk : Perk
{
    [Space, Header("Required Variables")]
    public float newCooldown;

    public override void Procced()
    {
        affectedAbility.abilityCooldown = newCooldown;
    }
}
