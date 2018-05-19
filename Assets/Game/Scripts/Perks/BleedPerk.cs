using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedPerk : Perk
{
    [Space, Header("Required Variables")]
    public int damagePerTick;
    public float bleedLength;

    public override void Procced(StatusEffects affected)
    {
        affected.Bleed(bleedLength, damagePerTick);
    }
}
