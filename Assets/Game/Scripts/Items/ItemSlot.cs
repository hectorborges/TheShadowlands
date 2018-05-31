using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public LootTable lootTable;
    public Text slotName;
    public Image slotIcon;

    public Text descriptionBoxName;
    public Text descriptionBox;
    public List<Text> statBoxes;

    public Text equippedDescriptionBoxName;
    public Text equippedDescriptionBox;
    public List<Text> equippedStatBoxes;

    [Space, Header("Ability Slots Information")]
    public List<Text> abilitySlotNames;
    public List<Image> abilitySlotIcons;

    public Item itemInSlot;

    public void UpdateItem(Item newItem)
    {
        itemInSlot = newItem;

        slotName.text = newItem.itemName;
        slotName.color = ItemTemplate.instance.GetItemRarityColor(newItem.itemRarity.ToString());
        slotIcon.sprite = newItem.itemIcon;
    }

    public void EquipItem(int itemSlot)
    {
        bool firstItemInSlot = false;

        if (PlayerLoadout.instance.itemsInSlots[itemSlot] == null)
            firstItemInSlot = true;

        Item oldEquippedItem = PlayerLoadout.instance.itemsInSlots[itemSlot];
        if (oldEquippedItem)
        {
            slotName.text = oldEquippedItem.itemName;
            slotName.color = ItemTemplate.instance.GetItemRarityColor(oldEquippedItem.itemRarity.ToString());
            slotIcon.sprite = oldEquippedItem.itemIcon;
        }

        if(PlayerLoadout.instance.itemsInSlots[itemSlot])
            print("New Item's Rarity is " + PlayerLoadout.instance.itemsInSlots[itemSlot].itemRarity);
        print("Old Item's Rarity is " + itemInSlot.itemRarity);
        lootTable.SwitchItems(PlayerLoadout.instance.itemsInSlots[itemSlot], itemInSlot, firstItemInSlot);

        if (PlayerLoadout.instance.itemsInSlots[itemSlot] == itemInSlot) return;
            PlayerLoadout.instance.EquipItem(itemInSlot, itemSlot);

        //if(firstItemInSlot)
        //    lootTable.RemoveEssenceWorth(this);

        lootTable.UpdateAbilitySlots(itemSlot, itemInSlot);

        if(oldEquippedItem)
            UpdateItem(oldEquippedItem);
        if (firstItemInSlot)
            gameObject.SetActive(false);
    }

    public void UpdateAbilitySlots(int itemSlot, Item item)
    {
        abilitySlotNames[itemSlot].text = item.itemName;
        abilitySlotNames[itemSlot].color = ItemTemplate.instance.GetItemRarityColor(item.itemRarity.ToString());
        abilitySlotIcons[itemSlot].sprite = item.itemIcon;
    }

    public void ViewDescription()
    {
        descriptionBoxName.color = ItemTemplate.instance.GetItemRarityColor(itemInSlot.itemRarity.ToString());
        descriptionBoxName.text = itemInSlot.itemName;

        for (int i = 0; i < statBoxes.Count; i++)
            statBoxes[i].text = "";

        for (int i = 0; i < itemInSlot.itemStats.Count; i++)
            statBoxes[i].text = ParseValue(itemInSlot.itemStats[i]);
        
        descriptionBoxName.transform.parent.gameObject.SetActive(true);

        //Equipped Item
        PlayerLoadout playerLoadout = PlayerLoadout.instance;
        Item equippedItem = null;
        for(int i = 0; i < playerLoadout.itemsInSlots.Count; i++)
        {
            if(playerLoadout.itemsInSlots[i] && itemInSlot)
                if (playerLoadout.itemsInSlots[i].itemName == itemInSlot.itemName)
                    equippedItem = playerLoadout.itemsInSlots[i];
        }

        if (equippedItem == null) return;
        print("Item in this slot is " + equippedItem);
        equippedDescriptionBoxName.color = ItemTemplate.instance.GetItemRarityColor(equippedItem.itemRarity.ToString());
        equippedDescriptionBoxName.text = equippedItem.itemName;

        for (int i = 0; i < equippedStatBoxes.Count; i++)
            equippedStatBoxes[i].text = "";

        for (int i = 0; i < equippedItem.itemStats.Count; i++)
        {
            equippedStatBoxes[i].text = ParseValue(equippedItem.itemStats[i]);
        }

        if (itemInSlot)
            equippedDescriptionBoxName.transform.parent.gameObject.SetActive(true);
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

    public void StopViewingDescription()
    {
        descriptionBoxName.transform.parent.gameObject.SetActive(false);
        equippedDescriptionBoxName.transform.parent.gameObject.SetActive(false);
    }
}
