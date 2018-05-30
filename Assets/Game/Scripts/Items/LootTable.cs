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

    public void RemoveItem(Item itemToRemove)
    {
        itemsInLootTable.Remove(itemToRemove);
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
