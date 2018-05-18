using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnPerk : Perk
{
    [Space, Header("Required Variables")]
    public int damagePerTick;
    public float burnLength;

    public override void Procced(StatusEffects affected)
    {
        affected.Burn(burnLength, damagePerTick);
    }
}
