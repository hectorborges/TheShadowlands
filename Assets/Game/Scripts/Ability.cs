using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Reference Document: https://docs.google.com/document/d/1ej2umWn3NffmtfLmVdlEdoDBSNRQDwEVvuF_01EiMzw/edit
namespace AbilitySystem
{
    public class Ability : MonoBehaviour
    {
        [Space, Header("Basic Information")]
        public string abilityName;
        public Sprite abilityIcon;
        public string abilityDescription;


        public enum InputType
        {
            GetButtonDown,
            GetButton,
            GetButtonUp
        };

        [Space, Header("Custom Variables")]
        public int abilityMinimumDamage;
        public int abilityMaximumDamage;
        public float abilityRange;
        public float abilityCastTime;
        public float abilityCooldown;
        public int abilityCharges;
        public int manaGained;
        public int manaRequired;

        public bool requiresTarget;

        [Space, Header("Required Variables")]
        public PlayerAnimator playerAnimator;
        public int abilityNumberOfAnimations;
        [Space]
        public int abilityDropChance;

        [Space, Header("Optional Variables")]
        public Weapon abilityWeapon;
        public List<Stat> abilityStats;
        public List<Perk> abilityPerks;

        [Space, Header("Camera Shake")]
        public float magnitude = 4f;
        public float roughness = 4f;
        public float fadeIn = .1f;
        public float fadeOut = 1f;

        //public List<Enchant> abilityEnchants;

        int currentCharges;
        int currentCooldown;
        bool onCooldown;

        public void Start()
        {
            if (abilityCharges <= 0)
                abilityCharges = 1;

            currentCharges = abilityCharges;
        }

        public virtual void ActivateAbility()
        {
            if (currentCharges >= 0)
        }
    }
}
