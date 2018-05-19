using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectilePerk : Perk
{
    [Space, Header("Required Variables")]
    public ObjectPooling projectile;

    private void Start()
    {
        projectile = ReferenceManager.rifleHitEffectPool;
    }

    public override void Procced()
    {
        print("Enable Explosive Projectiles");
        foreach(Collider col in projectile.pooledObjectsColliders)
        {
            col.enabled = true;
        }
    }
}
