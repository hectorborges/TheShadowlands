using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    [TextArea]
    public string itemDescription;

    public enum Rarity
    {
        Common,
        Rare,
        Epic,
        Legendary,
        Exotic,
        Artifact
    };

    public Rarity rarity;
    public List<Stat> stats;

    [Space, Header("Loot Type")]
    public Weapon weapon;
    public Ability ability;
    //public Scroll ability;
    //public Buff buff;
    //public Rune rune;

    Stats playerStats;

    ItemStats itemStats;

    public void Initialize()
    {
        ItemStats.instance.RollStats(this);
    }

    public void ApplyStats()
    {
        if(playerStats == null)
            playerStats = ReferenceManager.player.GetComponent<Stats>();

        foreach(Stat stat in stats)
        {
            playerStats.IncreaseStatBaseValue(stat.statType, stat.statCurrentValue);
            playerStats.IncreaseStatCurrentValue(stat.statType, stat.statCurrentValue);
        }
    }

    public void AddStat(Stat newStat)
    {
        stats.Add(newStat);
        print(" Amount of stats " + stats.Count);
    }
}
