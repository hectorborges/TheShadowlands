using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
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

    [HideInInspector] public float weaponExperience;
    [HideInInspector] public float currentWeaponLevel;

    public float requiredExperience;
    public float requiredExperienceMultiplier;
    public float maxWeaponLevel;

    public WeaponsVault weaponsVault;

    public enum ItemType
    {
        Axe,
        Swords,
        Shield,
        Rifle,
        Magic
    };

    [Space]
    public AnimatorOverrideController animatorOverrideController;

    private void Start()
    {
        if (currentWeaponLevel <= 0)
            currentWeaponLevel = 1;
    }

    public void GainExperience(float experience)
    {
        if(weaponExperience + experience < requiredExperience)
        {
            weaponExperience += experience;
            weaponsVault.SetExperience(itemType.ToString(), weaponExperience, requiredExperience);
        }
        else
        {
            float neededExperience = requiredExperience - weaponExperience;
            float excessExperience = experience - neededExperience;

            weaponExperience += neededExperience;
            weaponsVault.SetExperience(itemType.ToString(), weaponExperience, requiredExperience);

            weaponExperience = 0;
            weaponsVault.SetExperience(itemType.ToString(), weaponExperience, requiredExperience);

            currentWeaponLevel++;

            weaponExperience = excessExperience;
            requiredExperience *= requiredExperienceMultiplier;
            weaponsVault.SetExperience(itemType.ToString(), weaponExperience, requiredExperience);

        }
    }
}
