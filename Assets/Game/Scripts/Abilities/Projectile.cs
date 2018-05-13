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
    

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        hitEffects = ReferenceManager.rifleHitEffectPool;
    }

    public void SetDamage(int _minimumDamage, int _maximumDamage)
    {
        minimumDamage = _minimumDamage;
        maximumDamage = _maximumDamage;
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
            print(other.name);
            SpawnHitEffect();
            int randomDamage = Random.Range(minimumDamage, maximumDamage);
            other.GetComponent<EnemyHealth>().TookDamage(randomDamage);

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
    }
}
