using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    int essence;
    int totalEssenceWorth;

    List<Item> itemsInLootTable = new List<Item>();

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
        for (int i = 0; i < itemsInLootTable.Count; i++)
            Destroy(itemsInLootTable[i].gameObject);

        itemsInLootTable.Clear();
        totalEssenceWorth = 0;
        lootWindowEssenceWorth.text = totalEssenceWorth + " Essence";
        lootWindow.SetActive(true);

        for (int k = 0; k < itemSlots.Count; k++)
            itemSlots[k].gameObject.SetActive(false);

        int itemsDropped = Random.Range(1, 4);

        for (int j = 0; j < itemsDropped; j++)
            itemSlots[j].gameObject.SetActive(true);

        for(int i = 0; i < itemsDropped; i++)
        {
            Item randomItem = lootTable[Random.Range(0, lootTable.Length)];
            int dropChance = Random.Range(0, 100);

            if(dropChance <= randomItem.itemDropChance)
            {
                Item newItem = Instantiate(randomItem);
                newItem.itemAbility = randomItem.itemAbility;
                newItem.CreateItemStats();
                itemSlots[i].UpdateItem(newItem);
                itemsInLootTable.Add(newItem);
            }
            else
            {
                i--;
            }
        }
        totalEssenceWorth = 0;
        foreach (Item item in itemsInLootTable)
            totalEssenceWorth += ItemTemplate.instance.GetEssenceRarity(item.itemRarity.ToString());

        lootWindowEssenceWorth.text = totalEssenceWorth + " Essence";
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

    public void SwitchItems(Item itemInSlot, Item itemToRemove)
    {
        totalEssenceWorth -= ItemTemplate.instance.GetEssenceRarity(itemToRemove.itemRarity.ToString());
        totalEssenceWorth -= ItemTemplate.instance.GetEssenceRarity(itemInSlot.itemRarity.ToString());
        lootWindowEssenceWorth.text = totalEssenceWorth + " Essence";

        itemsInLootTable[itemsInLootTable.IndexOf(itemToRemove)] = itemInSlot;

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
