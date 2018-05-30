using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbility : Ability
{
    public ObjectPooling bullet;
    public List<GameObject> spawnpoints;

    [Space]
    public int minimumDamage;
    public int maximumDamage;
    public bool multifire;
    public bool alternateFire;

    PlayerMovement playerMovement;

    bool alternated;
    int currentAttack;
    int lastAttack;

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        TriggerCooldown();
    }

    public void Fire()
    {
        if(!multifire)
        {
            if(alternateFire)
            {
                lastAttack = currentAttack;
                if (currentAttack + 1 < abilityNumberOfAnimations)
                    currentAttack++;
                else
                    currentAttack = 0;

                if (!alternated)
                {
                    alternated = true;
                    LauncherProjectile(spawnpoints[0].transform);
                }
                else
                {
                    alternated = false;
                    LauncherProjectile(spawnpoints[1].transform);
                }
            }
            else
            {
                LauncherProjectile(spawnpoints[0].transform);
            }
        }
        else
        {
            for(int i = 0; i < spawnpoints.Count; i++)
            {
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

        projectile.SetDamage(minimumDamage, maximumDamage, this);

        if (obj == null)
        {
            return;
        }
        
        obj.transform.position = spawnPosition.position;
        obj.transform.rotation = spawnPosition.rotation;
        obj.SetActive(true);
    }

    public override void Animate()
    {
        if (playerAnimator)
        {
            playerAnimator.SpecificAttack(abilitySlot, currentAttack + 1);
        }
    }
}
