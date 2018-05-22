using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsVault : MonoBehaviour
{
    public PerksPage[] perksPages;
    public Image[] pageSelections;

    public Color unselectedColor;
    public Color selectedColor;

    public Image experienceBar;
    public Image skillBarExperienceBar;

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
        weapons.Add("Magic");

        for(int i = 0; i < weapons.Count; i++)
        {
            weaponsExperience.Add(0);
            weaponsMaxExperience.Add(0);
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
