using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public List<Stat> stats;
    Health health;
    Mana mana;

    private void Awake()
    {
        foreach (Stat stat in stats)
            stat.InitializeStat();

        health = GetComponent<Health>();
        mana = GetComponent<Mana>();
    }

    public void IncreaseStatCurrentValue(Stat.StatType statType, float value)
    {
        GetStat(statType).IncreaseCurrentValue(value);

        if(statType == Stat.StatType.Health && health)
            health.GainHealth((int)value);
        else if (statType == Stat.StatType.Mana && mana)
            mana.GainMana((int)value);
    }

    public void DecreaseStatCurrentValue(Stat.StatType statType, float value)
    {
        GetStat(statType).DecreaseCurrentValue(value);
    }

    public void IncreaseStatBaseValue(Stat.StatType statType, float value)
    {
        GetStat(statType).IncreaseBaseValue(value);

        if (statType == Stat.StatType.Health && health)
            health.UpdateBaseHealth();
        else if (statType == Stat.StatType.Mana && mana)
            mana.UpdateBaseMana();
    }

    public void DecreaseStatBaseValue(Stat.StatType statType, float value)
    {
        GetStat(statType).DecreaseBaseValue(value);
    }

    public float GetStatCurrentValue(Stat.StatType statType)
    {
        return GetStat(statType).GetCurrentValue();
    }

    public float GetStatBaseValue(Stat.StatType statType)
    {
        return GetStat(statType).GetBaseValue();
    }

    Stat GetStat(Stat.StatType statType)
    {
        foreach (Stat stat in stats)
        {
            if (stat.statType == statType)
                return stat;
        }
        return null;
    }
}
