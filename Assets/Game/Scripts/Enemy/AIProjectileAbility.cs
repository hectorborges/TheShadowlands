using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIProjectileAbility : Ability
{
    public ObjectPooling bullet;
    public Transform spawnPosition;

    PlayerMovement playerMovement;
    Transform player;

    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
    }

    private void Update()
    {
        if(!player)
            player = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        Fire();
        TriggerCooldown();
    }

    void Fire()
    {
        GameObject obj = bullet.GetPooledObject();

        Projectile projectile = obj.GetComponent<Projectile>();
        
        projectile.SetTarget(player);

        projectile.SetDamage(abilityMinimumDamage, abilityMaximumDamage, this);

        if (obj == null)
        {
            return;
        }

        obj.transform.position = spawnPosition.position;
        obj.transform.rotation = spawnPosition.rotation;
        obj.SetActive(true);
    }
}
