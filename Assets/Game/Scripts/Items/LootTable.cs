using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LootTable : MonoBehaviour
{
    public static LootTable instance;

    public Item[] lootTable;
    public Canvas playerCanvas;
    public GameObject lootWindow;
    public List<ItemSlot> itemSlots;
    public Text lootWindowEssenceWorth;
    public Text essenceText;

    [Space, Header("Equipped Ability Information")]
    public Text equippedAbilityName;
    public Text equippedAbilityDescription;
    public List<Text> equippedAbilityStats;

    [Space, Header("Item Drop Chances")]
    public List<int> itemDropCounts;
    public List<int> itemDropCountChances;

    [Space]
    int essence;
    int totalEssenceWorth;

    public List<Item> itemsInLootTable = new List<Item>();
    public List<int> essenceInLootTable = new List<int>();
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerCanvas.worldCamera = ReferenceManager.mainCamera;
    }

    public void NewLootTable()
    {
        //for (int i = 0; i < itemsInLootTable.Count; i++)
        //    Destroy(itemsInLootTable[i].gameObject);

        itemsInLootTable.Clear();
        essenceInLootTable.Clear();

        for(int l = 0; l < 3; l++)
        {
            itemsInLootTable.Add(null);
            essenceInLootTable.Add(0);
        }

        totalEssenceWorth = 0;
        lootWindowEssenceWorth.text = totalEssenceWorth + " Essence";
        lootWindow.SetActive(true);

        for (int k = 0; k < itemSlots.Count; k++)
            itemSlots[k].gameObject.SetActive(false);

        int itemsDropped = ItemsDropped();

        for (int j = 0; j < itemsDropped; j++)
            itemSlots[j].gameObject.SetActive(true);

        for(int i = 0; i < itemsDropped; i++)
        {
            Item randomItem = lootTable[Random.Range(0, lootTable.Length)];
            int dropChance = Random.Range(0, 100);

            if(dropChance <= randomItem.itemDropChance)
            {
                Item newItem = Instantiate(randomItem);
                newItem.itemRarity = randomItem.itemRarity;
                newItem.itemAbility = randomItem.itemAbility;
                newItem.CreateItemStats();
                itemSlots[i].UpdateItem(newItem);

                for(int m = 0; m < itemsInLootTable.Count; m++)
                {
                    if (itemsInLootTable[m] == null)
                    {
                        itemsInLootTable[m] = newItem;
                        essenceInLootTable[m] = ItemTemplate.instance.GetEssenceRarity(itemsInLootTable[m].itemRarity.ToString()); 
                        break;
                    }
                }
            }
            else
            {
                i--;
            }
        }
        totalEssenceWorth = 0;
        UpdateEssenceValue();
    }

    int ItemsDropped()
    {
        int itemsDroped = Random.Range(1, 4);
        int randomRoll = Random.Range(0, 101);

        if (randomRoll <= itemDropCountChances[itemDropCounts.IndexOf(itemsDroped)])
            return itemsDroped;
        else
            return ItemsDropped();
    }

    public void UpdateAbilitySlots(int abilitySlot, Item item)
    {
        foreach (ItemSlot itemSlot in itemSlots)
            itemSlot.UpdateAbilitySlots(abilitySlot, item);
    }

    public void ShowEquippedAbilityDescription(int abilityIndex)
    {
        Item equippedItem = PlayerLoadout.instance.itemsInSlots[abilityIndex];
        if (!equippedItem) return;
        equippedAbilityName.color = ItemTemplate.instance.GetItemRarityColor(equippedItem.itemRarity.ToString());
        equippedAbilityName.text = equippedItem.itemName;

        for (int i = 0; i < equippedAbilityStats.Count; i++)
            equippedAbilityStats[i].text = "";

        for (int i = 0; i < equippedItem.itemStats.Count; i++)
            equippedAbilityStats[i].text = ParseValue(equippedItem.itemStats[i]);

        equippedAbilityName.transform.parent.gameObject.SetActive(true);
    }

    public void StopShowingEquippedAbilityDescription()
    {
        equippedAbilityName.transform.parent.gameObject.SetActive(false);
    }

    string ParseValue(Stat stat)
    {
        switch (stat.statType)
        {
            case Stat.StatType.CriticalStrike:
                return Mathf.RoundToInt(stat.GetCurrentValue()) + "% Critical Strike";
            case Stat.StatType.CriticalDamage:
                return Mathf.RoundToInt((stat.GetCurrentValue() * 100)) + "% Critical Damage";
            case Stat.StatType.Damage:
                return Mathf.RoundToInt(stat.GetCurrentValue()) + " Damage";
            case Stat.StatType.Health:
                return Mathf.RoundToInt(stat.GetCurrentValue()) + " Health";
            case Stat.StatType.HealthPerHit:
                return Mathf.RoundToInt(stat.GetCurrentValue()) + " Health On Hit";
            case Stat.StatType.Mana:
                return Mathf.RoundToInt(stat.GetCurrentValue()) + " Mana";
            case Stat.StatType.ManaPerHit:
                return Mathf.RoundToInt(stat.GetCurrentValue()) + " Mana On Hit";
            default:
                return "";
        }
    }

    //public void RemoveEssenceWorth(ItemSlot itemSlot)
    //{
    //    int itemSlotIndex = itemSlots.IndexOf(itemSlot);
    //    print("Essence in Loot Table Index = " + itemSlotIndex);
    //    essenceInLootTable.ToArray()[itemSlotIndex] = 0;
    //    print("Essence in this slot is : " + essenceInLootTable.ToArray()[itemSlotIndex]);
    //    UpdateEssenceValue();
    //}

    void UpdateEssenceValue()
    {
        totalEssenceWorth = 0;
        foreach (int essenceWorth in essenceInLootTable)
            totalEssenceWorth += essenceWorth;
        lootWindowEssenceWorth.text = totalEssenceWorth + " Essence";
    }

    public void SwitchItems(Item newItem, Item oldItem, bool firstItemInSlot)
    {
        lootWindowEssenceWorth.text = totalEssenceWorth + " Essence";

        if (!firstItemInSlot)
        {
            int tempEssence = essenceInLootTable[itemsInLootTable.IndexOf(oldItem)];
            print("Temp Essence Before: " + tempEssence);
            tempEssence = ItemTemplate.instance.GetEssenceRarity(newItem.itemRarity.ToString());
            print("Temp Essence After: " + tempEssence);
            essenceInLootTable[itemsInLootTable.IndexOf(oldItem)] = tempEssence;
            
            Item tempItem = itemsInLootTable[itemsInLootTable.IndexOf(oldItem)];
            tempItem = newItem;
            itemsInLootTable[itemsInLootTable.IndexOf(oldItem)] = tempItem;
        }
        else
        {
            int tempEssence = essenceInLootTable[itemsInLootTable.IndexOf(oldItem)];
            tempEssence = 0;
            essenceInLootTable[itemsInLootTable.IndexOf(oldItem)] = tempEssence;

            Item tempItem = itemsInLootTable[itemsInLootTable.IndexOf(oldItem)];
            tempItem = null;
            itemsInLootTable[itemsInLootTable.IndexOf(oldItem)] = tempItem;
        }

        UpdateEssenceValue();

        if (!newItem) return;
        equippedAbilityName.color = ItemTemplate.instance.GetItemRarityColor(newItem.itemRarity.ToString());
        equippedAbilityName.text = newItem.itemName;

        for (int i = 0; i < equippedAbilityStats.Count; i++)
            equippedAbilityStats[i].text = "";

        for (int i = 0; i < newItem.itemStats.Count; i++)
            equippedAbilityStats[i].text = ParseValue(newItem.itemStats[i]);

        if (itemsInLootTable.Count <= 0)
            CloseLootWindow();
    }

    public void CloseLootWindow()
    {
        if (itemsInLootTable.Count > 0)
        {
            ReferenceManager.essenceAnimator.SetTrigger("Essence");
            StartCoroutine(CloseWindow());
            //warningMessage.SetActive(true);
        }
        else
            lootWindow.SetActive(false);
    }

    IEnumerator CloseWindow()
    {
        yield return new WaitForSeconds(1);
        essence += totalEssenceWorth;
        essenceText.text = essence.ToString();
        lootWindow.SetActive(false);
    }
}
