using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public bool targeted;

    int minimumDamage;
    int maximumDamage;
    Transform target;
    ObjectPooling hitEffects;
    Ability ability;
    

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        hitEffects = ReferenceManager.rifleHitEffectPool;
    }

    public void SetDamage(int _minimumDamage, int _maximumDamage, Ability _ability)
    {
        minimumDamage = _minimumDamage;
        maximumDamage = _maximumDamage;
        ability = _ability;
    }

    private void Update()
    {
        if (targeted && target)
        {
            transform.LookAt(target.transform.position);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else if(!targeted)
            transform.position += transform.forward * speed * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            SpawnHitEffect();
            ability.DealDamage(minimumDamage, maximumDamage, other.gameObject);

            gameObject.SetActive(false);
        }
        else if (other.tag.Equals("Environment"))
        {
            SpawnHitEffect();
            gameObject.SetActive(false);
        }
    }

    void SpawnHitEffect()
    {
        GameObject obj = hitEffects.GetPooledObject();

        if (obj == null)
        {
            return;
        }


        obj.transform.position = transform.position;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);

        obj.GetComponent<DamageTrigger>().SetAbility(ability);
    }
}
