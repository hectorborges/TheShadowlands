using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereColliderIncreasePerk : Perk
{
    [Space, Header("Required Variables")]
    public SphereCollider sphereCollider;
    public float colliderRadius;

    public override void Procced()
    {
        sphereCollider.radius = colliderRadius;
    }
}
