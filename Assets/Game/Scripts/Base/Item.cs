using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    [Space]
    public GameObject itemModel;
    public GameObject secondaryModel;
    public GameObject equipLocation;
    public GameObject secondaryEquipLocation;
    [Space]
    public Ability primaryAbility;
    public Ability secondaryAbility;

    public enum ItemType
    {
        Axe,
        Crossbow,
        DualSwords,
        Pistols,
        Rifle,
        Shield,
        Spear,
        Spell,
        Sword
    };
    [Space]
    public AnimatorOverrideController animatorOverrideController;
}
