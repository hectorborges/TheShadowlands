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

        descriptionBoxName.text = itemInSlot.itemName;
        descriptionBoxName.color = LootTable.instance.GetItemRarityColor(itemInSlot.rarity.ToString());

        descriptionBox.text = itemInSlot.itemDescription;

        for(int i = 0; i < itemInSlot.stats.Count; i++)
        {
            print(itemInSlot.stats[i].statType.ToString() + " " + itemInSlot.stats[i].GetCurrentValue());
            statBoxes[i].text = itemInSlot.stats[i].statType.ToString() + " " + itemInSlot.stats[i].GetCurrentValue();
        }
    }

    public void EquipItem()
    {
        LootTable.instance.RemoveItem();
    }

    public void ViewDescription()
    {
        if(itemInSlot)
            descriptionBox.transform.parent.gameObject.SetActive(true);       
    }

    public void StopViewingDescription()
    {
        if (itemInSlot)
            descriptionBox.transform.parent.gameObject.SetActive(false);
    }
}
