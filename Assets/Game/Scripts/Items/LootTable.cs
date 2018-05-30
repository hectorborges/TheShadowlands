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
    public GameObject warningMessage;
    public List<ItemSlot> itemSlots;

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

        lootWindow.SetActive(true);
        warningMessage.SetActive(false);

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
    }

    public void RemoveItem(Item itemToRemove)
    {
        itemsInLootTable.Remove(itemToRemove);
        if (itemsInLootTable.Count <= 0)
            CloseLootWindow();
    }

    public void CloseLootWindow()
    {
        if(itemsInLootTable.Count > 0)
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
        lootWindow.SetActive(false);
    }
}
