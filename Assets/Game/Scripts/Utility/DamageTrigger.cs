using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    public int minimumDamage;
    public int maximumDamage;

    Ability ability;

    public void SetDamage(int _minimumDamage, int _maximumDamage, Ability _ability)
    {
        minimumDamage = _minimumDamage;
        maximumDamage = _maximumDamage;
        ability = _ability;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Enemy"))
        {
            ability.DealDamage(minimumDamage, maximumDamage, other.gameObject);
        }
    }
}
