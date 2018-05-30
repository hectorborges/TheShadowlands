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

    Item itemInSlot;

    public void UpdateItem(Item newItem)
    {
        itemInSlot = newItem;

        slotName.text = itemInSlot.itemName;
        slotName.color = ItemTemplate.instance.GetItemRarityColor(itemInSlot.itemRarity.ToString());
        slotIcon.sprite = itemInSlot.itemIcon;
    }

    public void EquipItem()
    {
        PlayerLoadout.instance.EquipItem(itemInSlot, 0);

        print(itemInSlot.itemName);
        lootTable.RemoveItem(itemInSlot);
        itemInSlot = null;
        gameObject.SetActive(false);
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

        //if (weaponsVault.GetEquippedItem(itemInSlot.weapon) == null) return;
        //equippedDescriptionBoxName.color = ItemTemplate.instance.GetItemRarityColor(weaponsVault.GetEquippedItem(itemInSlot.weapon).rarity.ToString());
        //equippedDescriptionBoxName.text = weaponsVault.GetEquippedItem(itemInSlot.weapon).itemName;

        //for (int i = 0; i < equippedStatBoxes.Count; i++)
        //    equippedStatBoxes[i].text = "";

        //for (int i = 0; i < weaponsVault.GetEquippedItem(itemInSlot.weapon).stats.Count; i++)
        //{
        //    equippedStatBoxes[i].text = ParseValue(weaponsVault.GetEquippedItem(itemInSlot.weapon).stats[i]);
        //}

        //if (itemInSlot)
        //    equippedDescriptionBoxName.transform.parent.gameObject.SetActive(true);

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
