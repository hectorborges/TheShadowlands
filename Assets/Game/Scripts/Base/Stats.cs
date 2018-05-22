using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public List<Stat> stats;

    private void Awake()
    {
        foreach (Stat stat in stats)
            stat.InitializeStat();
    }

    public void IncreaseStatCurrentValue(Stat.StatType statType, float value)
    {
        GetStat(statType).IncreaseCurrentValue(value);
    }

    public void DecreaseStatCurrentValue(Stat.StatType statType, float value)
    {
        GetStat(statType).DecreaseCurrentValue(value);
    }

    public void IncreaseStatBaseValue(Stat.StatType statType, float value)
    {
        GetStat(statType).IncreaseBaseValue(value);
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
