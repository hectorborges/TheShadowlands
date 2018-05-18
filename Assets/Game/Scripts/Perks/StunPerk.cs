using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunPerk : Perk
{
    [Space, Header("Required Variables")]
    public float stunLength;

    public override void Procced(StatusEffects affected)
    {
        affected.Stun(stunLength);
    }
}
