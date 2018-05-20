using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectilePerk : Perk
{
    [Space, Header("Required Variables")]
    public ObjectPooling[] projectile;

    private void Start()
    {
        if(affectedAbility.abilityType == Ability.AbilityType.Rifle)
            projectile[0] = ReferenceManager.rifleHitEffectPool;
        else if(affectedAbility.abilityType == Ability.AbilityType.Pistols)
            projectile[0] = ReferenceManager.pistolHitEffectPool;
    }

    public override void Procced()
    {
        print("Enable Explosive Projectiles");
        for(int i = 0; i < projectile.Length; i++)
        {
            foreach (Collider col in projectile[i].pooledObjectsColliders)
            {
                col.enabled = true;
            }
        }
    }
}
