using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk : MonoBehaviour
{
    public string perkName;
    public Sprite perkIcon;
    [TextArea]
    public string perkDescription;
    public Ability affectedAbility;
    [Range(0, 100)]
    public int procChance;
    public bool requiresStatusEffects;

    public enum PerkType
    {
        ProcOnAttack,
        ProcOnDamaged,
        ProcOnKill,
        Buff,
        PerkUpgrade
    };

    public PerkType perkType;
    public bool activated;

    public bool refreshOnEquip;

	public virtual void ActivatePerk(StatusEffects affected)
    {
        print(perkName + " Activated!!");
        float roll = Random.Range(0, 100);

        if(roll <= procChance)
        {
            Procced(affected);
        }
    }

    public virtual void ActivatePerk()
    {
        print(perkName + " Activated!!");
        float roll = Random.Range(0, 100);

        if (roll <= procChance)
        {
            Procced();
        }
    }

    public virtual void Procced(StatusEffects affected) { }
    public virtual void Procced() { }

    public void SetPerkActive(bool status)
    {
        activated = status;
    }
}
