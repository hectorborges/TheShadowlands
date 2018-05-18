using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityProcPerk : Perk
{
    public override void Procced()
    {
        affectedAbility.ResetCooldown();
    }
}
