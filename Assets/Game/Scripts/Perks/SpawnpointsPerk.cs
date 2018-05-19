using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnpointsPerk : Perk
{
    [Space, Header("Required Variables")]
    public List<GameObject> extraSpawnpoints;

    public override void Procced()
    {
        ProjectileAbility projectileAbility = (ProjectileAbility)affectedAbility;
        for (int i = 0; i < extraSpawnpoints.Count; i++)
        {
            projectileAbility.spawnpoints.Add(extraSpawnpoints[i]);
        }
    }
}
