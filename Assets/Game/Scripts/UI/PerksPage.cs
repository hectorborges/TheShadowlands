using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerksPage : MonoBehaviour
{
    [Header("Abilities & Perks")]
    public Ability primaryAbility;
    public Ability secondaryAbility;
    public List<Perk> perks;

    [Space, Header("Slots")]
    public Image primaryAbilitySlot;
    public Image secondaryAbilitySlot;
    public List<PerkSlot> perkSlots;

    [Space]
    public Text perkNameArea;
    public Text descriptionArea;

    public enum WeaponType
    {
        Axe,
        Swords,
        Shield,
        Rifle,
        Pistols
    };

    public WeaponType weaponType;

    public void Initialize()
    {
        primaryAbilitySlot.sprite = primaryAbility.abilityIcon;
        secondaryAbilitySlot.sprite = secondaryAbility.abilityIcon;

        for (int i = 0; i < perkSlots.Count; i++)
        {
            perkSlots[i].Initialize(perks[i], descriptionArea, perkNameArea);
        }
    }
}
