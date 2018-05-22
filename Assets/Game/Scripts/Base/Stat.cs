﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable, CreateAssetMenu(menuName = "Stat")]
public class Stat : ScriptableObject
{
    public enum StatType
    {
        AttackSpeed,
        CriticalStrike,
        CriticalDamage,
        Damage,
        Health,
        HealthPerHit,
        Mana,
        ManaPerHit
    };

    public StatType statType;

    public float statBaseValue;
    public float statCurrentValue;

    public void InitializeStat()
    {
        statCurrentValue = statBaseValue;
    }

    public void IncreaseCurrentValue(float value)
    {
        statCurrentValue += value;
    }

    public void DecreaseCurrentValue(float value)
    {
        statCurrentValue -= value;
    }

    public void IncreaseBaseValue(float value)
    {
        statBaseValue += value;
    }

    public void DecreaseBaseValue(float value)
    {
        statBaseValue -= value;
    }

    public float GetCurrentValue()
    {
        return statCurrentValue;
    }

    public float GetBaseValue()
    {
        return statBaseValue;
    }
}
