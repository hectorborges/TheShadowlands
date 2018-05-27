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
    public GameObject levelUpEffect;
    public int maxPerkLevel = 10;

    public enum ItemType
    {
        Axe,
        Swords,
        Shield,
        Rifle,
        Pistols
    };

    [Space]
    public AnimatorOverrideController animatorOverrideController;

    bool weaponActivated;

    int currentPerkLevel;

    private void Start()
    {
        if (currentWeaponLevel <= 0)
            currentWeaponLevel = 1;
    }

    public int GetWeaponLevel()
    {
        return (int)currentWeaponLevel;
    }

    void InitializeWeaponPerks()
    {
        //In here I need to make sure to reactivate all of the perks that are unlocked
        //for each weapon once I begin to add the saving system.
    }

    public void GainExperience(float experience)
    {

        if (weaponExperience + experience < requiredExperience)
        {
            if (currentWeaponLevel < maxWeaponLevel)
            {
                weaponExperience += experience;
                weaponsVault.SetExperience(itemType.ToString(), weaponExperience, requiredExperience);
            }
        }
        else
        {
            if(currentWeaponLevel < maxWeaponLevel)
            {
                float neededExperience = requiredExperience - weaponExperience;
                float excessExperience = experience - neededExperience;

                currentWeaponLevel++;

                if (currentWeaponLevel == maxWeaponLevel)
                {
                    weaponsVault.SetExperience(itemType.ToString(), weaponExperience, neededExperience);
                }
                else if (currentWeaponLevel < maxWeaponLevel)
                {
                    weaponExperience += neededExperience;
                    weaponsVault.SetExperience(itemType.ToString(), weaponExperience, requiredExperience);

                    weaponExperience = 0;
                    weaponsVault.SetExperience(itemType.ToString(), weaponExperience, requiredExperience);

                    levelUpEffect.SetActive(true);

                    weaponExperience = excessExperience;
                    requiredExperience *= requiredExperienceMultiplier;
                    weaponsVault.SetExperience(itemType.ToString(), weaponExperience, requiredExperience);
                }

                if(currentPerkLevel < maxWeaponLevel)
                {
                    for (int i = 0; i < weaponPerks[currentPerkLevel].perks.Length; i++)
                    {
                        weaponPerks[currentPerkLevel].perks[i].SetPerkActive(true);
                        if (weaponPerks[currentPerkLevel].perks[i].perkType == Perk.PerkType.Buff)
                        {
                            weaponPerks[currentPerkLevel].perks[i].ActivatePerk();
                        }
                    }

                    weaponPerks[currentPerkLevel].perkSlot.UnlockPerk();
                    currentPerkLevel++;
                }
            }
        }
    }

    public void SetWeaponActive(bool status)
    {
        ////The thorn effect is not getting reactivated.
        PlayerHealth.instance.SetImmunity(0);
        PlayerHealth.instance.SetThornsActive(false);

        for(int i = 0; i < currentPerkLevel; i++)
        {
            weaponPerks[i].perks[0].activated = status;

            if (weaponPerks[i].perks[0].activated && weaponPerks[i].perks[0].refreshOnEquip)
            {
                print("Reactivate " + weaponPerks[i].perks[0]);
                weaponPerks[i].perks[0].ActivatePerk();
            }
        }

        //weaponActivated = status;

        //for (int i = 1; i < currentWeaponLevel; i++)
        //{
        //    print("Perk Count: " + weaponPerks[i].perks.Length);
        //    for (int j = 0; j < weaponPerks[i].perks.Length; j++)
        //    {
        //        weaponPerks[i].perks[j].activated = weaponActivated;

        //        if (weaponPerks[i].perks[j].activated && weaponPerks[i].perks[j].refreshOnEquip)
        //        {
        //            weaponPerks[i].perks[j].ActivatePerk();
        //        }
        //    }
        //}
    }
}

[System.Serializable]
public class WeaponLevelPerks
{
    public Perk[] perks;
    public PerkSlot perkSlot;
}
