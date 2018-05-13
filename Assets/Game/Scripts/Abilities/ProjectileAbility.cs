using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbility : Ability
{
    public ObjectPooling bullet;
    public GameObject[] spawnpoints;

    [Space]
    public int minimumDamage;
    public int maximumDamage;
    public bool multifire;

    PlayerMovement playerMovement;

    public override void ActivateAbility()
    {
        base.ActivateAbility();

        PlayerMovement.canMove = false;
        TriggerCooldown();
    }

    public override void DeactivateAbility()
    {
        base.DeactivateAbility();

        PlayerMovement.canMove = true;
    }
    public void Fire()
    {
        if(!multifire)
        {
            LauncherProjectile(spawnpoints[0].transform);
        }
        else
        {
            for(int i = 0; i < spawnpoints.Length; i++)
            {
                print("FIRE");
                LauncherProjectile(spawnpoints[i].transform);
            }
        }
    }

    void LauncherProjectile(Transform spawnPosition)
    {
        GameObject obj = bullet.GetPooledObject();

        Projectile projectile = obj.GetComponent<Projectile>();

        if(PlayerLoadout.focus != null)
            projectile.SetTarget(PlayerLoadout.focus.transform);

        projectile.SetDamage(minimumDamage, maximumDamage);

        if (obj == null)
        {
            return;
        }
        
        obj.transform.position = spawnPosition.position;
        obj.transform.rotation = spawnPosition.rotation;
        obj.SetActive(true);
    }
}
