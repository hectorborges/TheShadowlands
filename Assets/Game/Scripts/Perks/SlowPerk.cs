using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPerk : Perk
{
    [Space, Header("Required Variables")]
    public float slowLength;

    public override void Procced(StatusEffects affected)
    {
        affected.Slow(slowLength);
    }
}
