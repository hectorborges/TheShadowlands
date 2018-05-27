﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : MonoBehaviour
{
    public static ItemStats instance;
    public List<Stat> stats;

    public float maxCriticalStrikeValue;
    public float maxCriticalDamageValue;
    public float maxDamageValue;
    public float maxHealthValue;
    public float maxHealthPerHitValue;
    public float maxManaValue;
    public float maxManaPerHitValue;

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

        criticalStike.maxValuePerItem = maxCriticalStrikeValue;
        criticalDamage.maxValuePerItem = maxCriticalDamageValue;
        damage.maxValuePerItem = maxDamageValue;
        health.maxValuePerItem = maxHealthPerHitValue;
        healthPerHit.maxValuePerItem = maxHealthPerHitValue;
        mana.maxValuePerItem = maxManaValue;
        manaPerHit.maxValuePerItem = maxManaPerHitValue;

        stats.Add(criticalStike);
        stats.Add(criticalDamage);
        stats.Add(damage);
        stats.Add(health);
        stats.Add(healthPerHit);
        stats.Add(mana);
        stats.Add(manaPerHit);
    }

    public void RollStats(Item item)
    {
        item.rarity = RandomRarity(item);
        int statCount = CheckRarity(item);

        for(int i = 0; i < statCount; i++)
        {
            Stat stat = stats[Random.Range(0, stats.Count)];

            stat.statCurrentValue = Random.Range(0, stat.maxValuePerItem);
            item.AddStat(stat);
        }
    }

    Item.Rarity RandomRarity(Item item)
    {
        int randomRarity = Random.Range(0, 6);

        switch (randomRarity)
        {
            case 0:
                return Item.Rarity.Common;
            case 1:
                return Item.Rarity.Rare;
            case 2:
                return Item.Rarity.Epic;
            case 3:
                return Item.Rarity.Legendary;
            case 4:
                return Item.Rarity.Exotic;
            case 5:
                return Item.Rarity.Artifact;
            default:
                return Item.Rarity.Common;
        }
    }

    public int CheckRarity(Item item)
    {
        switch (item.rarity)
        {
            case Item.Rarity.Common:
                return 0;
            case Item.Rarity.Rare:
                return 1;
            case Item.Rarity.Epic:
                return 2;
            case Item.Rarity.Legendary:
                return 3;
            case Item.Rarity.Exotic:
                return 4;
            case Item.Rarity.Artifact:
                return 5;
            default:
                return 0;
        }
    }
}
