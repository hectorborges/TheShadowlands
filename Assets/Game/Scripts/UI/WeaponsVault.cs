using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsVault : MonoBehaviour
{
    public LootTable lootTable;
    public Image[] weaponIcons;
    public Item[] equippedItems = new Item[5];
    public PerksPage[] perksPages;
    public Image[] pageSelections;

    public Color unselectedColor;
    public Color selectedColor;

    public Image experienceBar;
    public Image skillBarExperienceBar;

    public Text descriptionAreaName;
    public List<Text> statsArea;

    int currentPerksPageSelectedIndex;
    PerksPage currentPerksPageSelected;
    Image currentPageSelected;

    public List<string> weapons = new List<string>();
    List<float> weaponsExperience = new List<float>();
    List<float> weaponsMaxExperience = new List<float>();

    bool initialized;

    private void OnEnable()
    {
        if (!initialized)
            Initialize();
    }

    public void EquipWeapon(Item item)
    {
        PlayerLoadout.instance.EquippedItem(item);
        switch(item.weapon.itemName)
        {
            case "Axe":
                weaponIcons[0].sprite = item.weapon.itemIcon;
                weaponIcons[0].GetComponent<Button>().interactable = true;
                equippedItems[0] = item;
                break;
            case "Swords":
                weaponIcons[1].sprite = item.weapon.itemIcon;
                weaponIcons[1].GetComponent<Button>().interactable = true;
                equippedItems[1] = item;
                break;
            case "Shield":
                weaponIcons[2].sprite = item.weapon.itemIcon;
                weaponIcons[2].GetComponent<Button>().interactable = true;
                equippedItems[2] = item;
                break;
            case "Rifle":
                weaponIcons[3].sprite = item.weapon.itemIcon;
                weaponIcons[3].GetComponent<Button>().interactable = true;
                equippedItems[3] = item;
                break;
            case "Pistols":
                weaponIcons[4].sprite = item.weapon.itemIcon;
                weaponIcons[4].GetComponent<Button>().interactable = true;
                equippedItems[4] = item;
                break;
        }
    }

    public Item GetEquippedItem(Weapon weapon)
    {
        for(int i = 0; i < equippedItems.Length; i++)
        {
            if (equippedItems[i] != null && weapon.itemType == equippedItems[i].weapon.itemType)
                return equippedItems[i];
        }
        return null;
    }

    public void ViewDescription(int itemIndex)
    {
        if (weaponIcons[itemIndex].GetComponent<Button>().interactable == false)
            return;

        descriptionAreaName.color = lootTable.GetItemRarityColor(equippedItems[itemIndex].rarity.ToString());
        descriptionAreaName.text = equippedItems[itemIndex].itemName;

        for (int j = 0; j < statsArea.Count; j++)
            statsArea[j].text = "";

        for(int i = 0; i < equippedItems[itemIndex].stats.Count; i++)
        {
            statsArea[i].text = ParseValue(equippedItems[itemIndex].stats[i]);
        }
        descriptionAreaName.transform.parent.gameObject.SetActive(true);
    }
    
    public void StopViewingDescription()
    {
        descriptionAreaName.transform.parent.gameObject.SetActive(false);
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

    private void Initialize()
    {
        currentPerksPageSelected = perksPages[0];
        currentPageSelected = pageSelections[0];
        currentPerksPageSelectedIndex = 0;

        currentPerksPageSelected.gameObject.SetActive(true);
        currentPageSelected.color = selectedColor;

        weapons.Add("Axe");
        weapons.Add("Swords");
        weapons.Add("Shield");
        weapons.Add("Rifle");
        weapons.Add("Pistols");

        for(int i = 0; i < weapons.Count; i++)
        {
            weaponsExperience.Add(0);
            weaponsMaxExperience.Add(0);
        }

        foreach(PerksPage perksPage in perksPages)
        {
            perksPage.Initialize();
        }

        initialized = true;
    }

    public void SetExperience(string weapon, float experience, float maxExperience)
    {
        if (!initialized)
            Initialize();

        experienceBar.fillAmount = weaponsExperience[currentPerksPageSelectedIndex] / weaponsMaxExperience[currentPerksPageSelectedIndex];
        skillBarExperienceBar.fillAmount = experience / maxExperience;

        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapon == weapons[i])
            {
                weaponsExperience[i] = experience;
                weaponsMaxExperience[i] = maxExperience;
            }
        }
    }

    public void SetExperience(string weapon)
    {
        if (!initialized)
            Initialize();

        for (int i = 0; i < weapons.Count; i++)
        {
            if(weapon == weapons[i])
            {
                experienceBar.fillAmount = weaponsExperience[i] / weaponsMaxExperience[i];
                skillBarExperienceBar.fillAmount = weaponsExperience[i] / weaponsMaxExperience[i];
            }
        }
    }

    public void SelectPage(string pageSelected)
    {
        if (!initialized)
            Initialize();

        for (int i = 0; i < perksPages.Length; i++)
        {
            if(pageSelected == perksPages[i].weaponType.ToString())
            {
                currentPerksPageSelected.gameObject.SetActive(false);
                currentPageSelected.color = unselectedColor;

                currentPerksPageSelectedIndex = i;
                currentPerksPageSelected = perksPages[i];
                currentPageSelected = pageSelections[i];

                currentPerksPageSelected.gameObject.SetActive(true);
                currentPageSelected.color = selectedColor;
            }
        }
        SetExperience(pageSelected);
    }
}
