using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Text slotName;
    public Image slotIcon;

    public Text descriptionBoxName;
    public Text descriptionBox;

    public List<Text> statBoxes;

    Item itemInSlot;

    public void UpdateItem(Item newItem)
    {
        print("Updating Item");
        itemInSlot = newItem;

        slotName.name = itemInSlot.itemName;
        slotName.color = LootTable.instance.GetItemRarityColor(itemInSlot.rarity.ToString());
        slotIcon.sprite = itemInSlot.itemIcon;
    }

    public void EquipItem()
    {
        LootTable.instance.RemoveItem();
    }

    public void ViewDescription()
    {
        descriptionBoxName.color = LootTable.instance.GetItemRarityColor(itemInSlot.rarity.ToString());
        descriptionBoxName.text = itemInSlot.itemName;
        descriptionBox.text = itemInSlot.itemDescription;

        for (int i = 0; i < statBoxes.Count; i++)
            statBoxes[i].text = "";

        print("Stat Boxes >> " + statBoxes.Count);
        print("Item in slots >> " + itemInSlot.stats.Count);

        for (int i = 0; i < itemInSlot.stats.Count; i++)
        {
            statBoxes[i].text = itemInSlot.stats[i].statType.ToString() + " " + itemInSlot.stats[i].GetCurrentValue();
        }

        if (itemInSlot)
            descriptionBox.transform.parent.gameObject.SetActive(true);       
    }

    public void StopViewingDescription()
    {
        if (itemInSlot)
            descriptionBox.transform.parent.gameObject.SetActive(false);
    }
}
