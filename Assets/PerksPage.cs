using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerksPage : MonoBehaviour
{
    public enum WeaponType
    {
        Axe,
        Swords,
        Shield,
        Rifle,
        Magic
    };

    public WeaponType weaponType;
    public Image perkSlots;
}
