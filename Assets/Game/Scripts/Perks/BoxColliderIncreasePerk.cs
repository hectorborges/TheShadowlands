using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BocColliderIncreasePerk : Perk
{
    [Space, Header("Required Variables")]
    public BoxCollider boxCollider;
    public Vector3 colliderSize;

    public override void Procced(StatusEffects affected)
    {
        boxCollider.size = colliderSize;
    }
}
