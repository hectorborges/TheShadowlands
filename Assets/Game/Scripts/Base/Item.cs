using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/New Item")]
public class Item : ScriptableObject
{
    public GameObject itemModel;

    public enum ItemEquipType { mainHand, offHand, twoHanded, eitherHand };
    public ItemEquipType equipType;

    public enum ItemType
    {
        twoHandAxe,
        bow,
        twoHandClub,
        crossbow,
        twoHandSpear,
        twoHandSword,
        dagger,
        mace,
        rifle,
        shield,
        spear,
        spell,
        sword
    };

    public ItemType itemType;
    public GameObject itemEffect;
    public GameObject damageCollider;
    public Ability itemAbility;
    public AnimatorOverrideController animatorOverrideController;
    public Stat[] itemStats;
}

[System.Serializable]
public class Stat
{
    public enum StatType
    {
        minimumDamage,
        maximumDamage,
        attackSpeed,
        maxHealth
    };

    public StatType statType;

    public float statAmount;
}
