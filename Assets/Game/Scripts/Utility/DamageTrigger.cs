using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    public int minimumDamage;
    public int maximumDamage;

    public void SetDamage(int _minimumDamage, int _maximumDamage)
    {
        minimumDamage = _minimumDamage;
        maximumDamage = _maximumDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Enemy"))
        {
            print("Hit");
            int randomDamage = Random.Range(minimumDamage, maximumDamage);
            other.GetComponent<EnemyHealth>().TookDamage(randomDamage);
        }
    }
}
