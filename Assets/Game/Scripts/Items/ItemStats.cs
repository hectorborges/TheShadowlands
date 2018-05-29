using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is a template of all of the possible stats that an item can roll with.
//This class also covers the minimum and maximum possible values each stat can roll with.

public class ItemStatsTemplate : MonoBehaviour
{
    public static ItemStatsTemplate instance;

    [Space, Header("Ability Stats Constraints")]

    public float minCriticalStrikeValue;
    public float minCriticalDamageValue;
    public float minDamageValue;
    public float minHealthValue;
    public float minHealthPerHitValue;
    public float minManaValue;
    public float minManaPerHitValue;

    public float maxCriticalStrikeValue;
    public float maxCriticalDamageValue;
    public float maxDamageValue;
    public float maxHealthValue;
    public float maxHealthPerHitValue;
    public float maxManaValue;
    public float maxManaPerHitValue;

    [HideInInspector] public List<Stat> possibleStats = new List<Stat>();

    private void Awake()
    {
        instance = this;

        Stat criticalStike = new Stat();
        Stat criticalDamage = new Stat();
        Stat damage = new Stat();
        Stat health = new Stat();
        Stat healthPerHit = new Stat();
        Stat mana = new Stat();
        Stat manaPerHit = new Stat();

        criticalStike.statType = Stat.StatType.CriticalStrike;
        criticalDamage.statType = Stat.StatType.CriticalDamage;
        damage.statType = Stat.StatType.Damage;
        health.statType = Stat.StatType.Health;
        healthPerHit.statType = Stat.StatType.HealthPerHit;
        mana.statType = Stat.StatType.Mana;
        manaPerHit.statType = Stat.StatType.ManaPerHit;

        criticalStike.maxValuePerItem = maxCriticalStrikeValue;
        criticalDamage.maxValuePerItem = maxCriticalDamageValue;
        damage.maxValuePerItem = maxDamageValue;
        health.maxValuePerItem = maxHealthPerHitValue;
        healthPerHit.maxValuePerItem = maxHealthPerHitValue;
        mana.maxValuePerItem = maxManaValue;
        manaPerHit.maxValuePerItem = maxManaPerHitValue;

        criticalStike.minValuePerItem = minCriticalStrikeValue;
        criticalDamage.minValuePerItem = minCriticalDamageValue;
        damage.minValuePerItem = minDamageValue;
        health.minValuePerItem = minHealthPerHitValue;
        healthPerHit.minValuePerItem = minHealthPerHitValue;
        mana.minValuePerItem = minManaValue;
        manaPerHit.minValuePerItem = minManaPerHitValue;

        possibleStats.Add(criticalStike);
        possibleStats.Add(criticalDamage);
        possibleStats.Add(damage);
        possibleStats.Add(health);
        possibleStats.Add(healthPerHit);
        possibleStats.Add(mana);
        possibleStats.Add(manaPerHit);
    }
}
