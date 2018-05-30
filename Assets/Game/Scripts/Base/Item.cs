using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Ability itemAbility;
    public int itemDropChance;
    //public Scroll scrollItem;

    [HideInInspector] public string itemName;
    [HideInInspector] public Sprite itemIcon;
    [HideInInspector] public string itemDescription;

    public enum ItemRarity { Common, Rare, Epic, Legendary, Exotic, Artifact };
    [HideInInspector] public ItemRarity itemRarity;

    [HideInInspector] public List<Stat> itemStats = new List<Stat>();

    Stats playerStats;

    public void CreateItemStats()
    {
        ItemTemplate itemStatsTemplate = ItemTemplate.instance;

        if(itemAbility)
        {
            itemName = itemAbility.abilityName;
            itemIcon = itemAbility.abilityIcon;
            itemDescription = itemAbility.abilityDescription;
        }

        itemRarity = GetRandomRarity();                        
        int itemStatCount = GetChosenRarityIndex();

        //This loop picks a random stat template and creates a new stat in it's image. After doing so it adds the stat to this item's stats
        //This continues for how every many stats this item rolled with
        for (int i = 0; i < itemStatCount; i++)
        {
            Stat statTemplate = itemStatsTemplate.possibleStats[Random.Range(0, itemStatsTemplate.possibleStats.Count)];
            Stat stat = new Stat();
            stat.minValuePerItem = statTemplate.minValuePerItem;
            stat.maxValuePerItem = statTemplate.maxValuePerItem;
            stat.InitializeStat();
            stat.statCurrentValue = Random.Range(stat.minValuePerItem, stat.maxValuePerItem);
            itemStats.Add(stat);
        }
    }

    #region CheckMinimumRarity
    int CheckMinimumRarity()
    {
        switch (itemAbility.minimumRarity)
        {
            case Ability.MinimumRarity.Common:
                return 0;
            case Ability.MinimumRarity.Rare:
                return 1;
            case Ability.MinimumRarity.Epic:
                return 2;
            case Ability.MinimumRarity.Legendary:
                return 3;
            case Ability.MinimumRarity.Exotic:
                return 4;
            case Ability.MinimumRarity.Artifact:
                return 5;
            default:
                return 0;
        }
    }
    #endregion
    #region GetRandomRarity
    ItemRarity GetRandomRarity()
    {
        int randomRarity = Random.Range(CheckMinimumRarity(), 6);

        switch (randomRarity)
        {
            case 0:
                return ItemRarity.Common;
            case 1:
                return ItemRarity.Rare;
            case 2:
                return ItemRarity.Epic;
            case 3:
                return ItemRarity.Legendary;
            case 4:
                return ItemRarity.Exotic;
            case 5:
                return ItemRarity.Artifact;
            default:
                return ItemRarity.Common;
        }
    }
    #endregion
    #region GetChosenRarityIndex
    public int GetChosenRarityIndex()
    {
        switch (itemRarity)
        {
            case ItemRarity.Common:
                return 0;
            case ItemRarity.Rare:
                return 1;
            case ItemRarity.Epic:
                return 2;
            case ItemRarity.Legendary:
                return 3;
            case ItemRarity.Exotic:
                return 4;
            case ItemRarity.Artifact:
                return 5;
            default:
                return 0;
        }
    }
    #endregion
}
