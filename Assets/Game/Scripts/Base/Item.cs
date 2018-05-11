using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
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
    public GameObject[] itemEffects;
    public GameObject[] damageColliders;
    public Ability primaryAbility;
    public Ability secondaryAbility;
    public AnimatorOverrideController animatorOverrideController;
}
