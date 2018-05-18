using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk : MonoBehaviour
{
    public Ability affectedAbility;
    [Range(0, 100)]
    public int procChance;

	public virtual void ActivatePerk(StatusEffects affected)
    {
        float roll = Random.Range(0, 100);

        if(roll <= procChance)
        {
            Procced(affected);
        }
    }

    public virtual void Procced(StatusEffects affected) { }
}
