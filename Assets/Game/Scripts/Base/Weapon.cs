﻿using System.Collections;
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
    public WeaponLevelPerks[] weaponPerks;

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

    bool weaponActivated;

    private void Start()
    {
        if (currentWeaponLevel <= 0)
            currentWeaponLevel = 1;
    }

    void InitializeWeaponPerks()
    {
        //In here I need to make sure to reactivate all of the perks that are unlocked
        //for each weapon once I begin to add the saving system.
    }

    public void GainExperience(float experience)
    {
        if (currentWeaponLevel >= maxWeaponLevel) return;
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

            for(int i = 0; i < weaponPerks[(int)currentWeaponLevel].perks.Length; i++)
            {
                weaponPerks[(int)currentWeaponLevel].perks[i].SetPerkActive(true);
                Debug.Log("Current Level: " + currentWeaponLevel + "  --  Perk: " + weaponPerks[(int)currentWeaponLevel].perks[i].perkName);
                Debug.Log(" Perk Type : " + weaponPerks[(int)currentWeaponLevel].perks[i].perkType);
                if (weaponPerks[(int)currentWeaponLevel].perks[i].perkType == Perk.PerkType.Buff)
                {
                    weaponPerks[(int)currentWeaponLevel].perks[i].ActivatePerk();
                }
            }
        }
    }

    public void SetWeaponActive(bool status)
    {
        weaponActivated = status;

        for (int i = 1; i < (int)currentWeaponLevel; i++)
        {
            for (int j = 1; j < (int)currentWeaponLevel; j++)
            {
                weaponPerks[i].perks[j].activated = weaponActivated;
            }
        }
    }
}

[System.Serializable]
public class WeaponLevelPerks
{
    public Perk[] perks;
}
