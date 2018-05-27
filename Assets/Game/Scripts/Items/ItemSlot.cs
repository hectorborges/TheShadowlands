using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public LootTable lootTable;
    public WeaponsVault weaponsVault;
    public Text slotName;
    public Image slotIcon;

    public Text descriptionBoxName;
    public List<Text> statBoxes;

    public Text equippedDescriptionBoxName;
    public List<Text> equippedStatBoxes;

    Item itemInSlot;

    public void UpdateItem(Item newItem)
    {
        print("Updating Item");
        itemInSlot = newItem;

        slotName.text = itemInSlot.itemName;
        slotName.color = lootTable.GetItemRarityColor(itemInSlot.rarity.ToString());
        slotIcon.sprite = itemInSlot.itemIcon;
    }

    public void EquipItem()
    {
        if(itemInSlot.weapon)
            weaponsVault.EquipWeapon(itemInSlot);

        lootTable.RemoveItem();

        if (itemInSlot)
        {
            descriptionBoxName.transform.parent.gameObject.SetActive(false);
            equippedDescriptionBoxName.transform.parent.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    public void ViewDescription()
    {
        slotName.text = itemInSlot.itemName;
        slotName.color = lootTable.GetItemRarityColor(itemInSlot.rarity.ToString());
        slotIcon.sprite = itemInSlot.itemIcon;

        descriptionBoxName.color = lootTable.GetItemRarityColor(itemInSlot.rarity.ToString());
        descriptionBoxName.text = itemInSlot.itemName;

        for (int i = 0; i < statBoxes.Count; i++)
            statBoxes[i].text = "";

        print("Stat Boxes >> " + statBoxes.Count);
        print("Item in slots >> " + itemInSlot.stats.Count);

        for (int i = 0; i < itemInSlot.stats.Count; i++)
        {
            statBoxes[i].text = ParseValue(itemInSlot.stats[i]);
        }

        if (itemInSlot)
            descriptionBoxName.transform.parent.gameObject.SetActive(true);

        //Equipped Item

        if (weaponsVault.GetEquippedItem(itemInSlot.weapon) == null) return;
        equippedDescriptionBoxName.color = lootTable.GetItemRarityColor(weaponsVault.GetEquippedItem(itemInSlot.weapon).rarity.ToString());
        equippedDescriptionBoxName.text = weaponsVault.GetEquippedItem(itemInSlot.weapon).itemName;

        for (int i = 0; i < equippedStatBoxes.Count; i++)
            equippedStatBoxes[i].text = "";

        for (int i = 0; i < weaponsVault.GetEquippedItem(itemInSlot.weapon).stats.Count; i++)
        {
            equippedStatBoxes[i].text = ParseValue(weaponsVault.GetEquippedItem(itemInSlot.weapon).stats[i]);
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
        if (itemInSlot)
        {
            descriptionBoxName.transform.parent.gameObject.SetActive(false);
            equippedDescriptionBoxName.transform.parent.gameObject.SetActive(false);
        }
    }
}
