using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderIncreasePerk : Perk
{
    [Space, Header("Required Variables")]
    public BoxCollider boxCollider;
    public Vector3 colliderSize;

    public override void Procced()
    {
        boxCollider.size = colliderSize;
    }
}
