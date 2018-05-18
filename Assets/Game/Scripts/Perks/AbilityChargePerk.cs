using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityChargePerk : Perk
{
    [Space, Header("Required Variables")]
    public int chargeCount;

    public override void Procced(StatusEffects affected)
    {
        affectedAbility.charges = chargeCount;
    }
}
