using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledSphereColliderIncreasePerk : Perk
{
    [Space, Header("Required Variables")]
    public float colliderRadius;

    public ObjectPooling targetedProjectiles;
    public ObjectPooling unTargetedProjectiles;

    public override void Procced()
    {
        foreach (GameObject go in targetedProjectiles.pooledObject)
            go.GetComponent<SphereCollider>().radius = colliderRadius;

        foreach (GameObject go in unTargetedProjectiles.pooledObject)
            go.GetComponent<SphereCollider>().radius = colliderRadius;
    }
}
