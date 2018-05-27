using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootTable : MonoBehaviour
{
    public static LootTable instance;

    public Item[] lootTable;
    public GameObject lootWindow;
    public GameObject warningMessage;
    public List<ItemSlot> itemSlots;

    public List<string> rarities;
    public List<Color> rarityColors;

    int itemsInLootTable;

    private void Awake()
    {
        instance = this;
    }

    public Color GetItemRarityColor(string rarity)
    {
        if (rarities.Contains(rarity))
        {
            int rarityIndex = rarities.IndexOf(rarity);
            return rarityColors[rarityIndex];
        }
        else
            return Color.white;
    }

    public void NewLootTable()
    {
        itemsInLootTable = 0;
        lootWindow.SetActive(true);
        warningMessage.SetActive(false);

        for (int k = 0; k < itemSlots.Count; k++)
            itemSlots[k].gameObject.SetActive(false);

        int itemsDropped = Random.Range(1, 4);
        itemsInLootTable = itemsDropped;

        for (int j = 0; j < itemsDropped; j++)
            itemSlots[j].gameObject.SetActive(true);

        for(int i = 0; i < itemsDropped; i++)
        {
            Item randomItem = lootTable[Random.Range(0, lootTable.Length)];
            itemSlots[i].UpdateItem(randomItem);
        }
    }

    public void RemoveItem()
    {
        if (itemsInLootTable <= 0)
            CloseLootWindow();
    }

    public void CloseLootWindow()
    {
        if(itemsInLootTable > 0)
            warningMessage.SetActive(true);
        else
            lootWindow.SetActive(false);
    }
}
