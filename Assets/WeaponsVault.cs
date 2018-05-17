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

    PerksPage currentPerksPageSelected;
    Image currentPageSelected;

    private void Start()
    {
        currentPerksPageSelected = perksPages[0];
        currentPageSelected = pageSelections[0];

        currentPerksPageSelected.gameObject.SetActive(true);
        currentPageSelected.color = selectedColor;
    }

    public void SelectPage(string pageSelected)
    {
        for(int i = 0; i < perksPages.Length; i++)
        {
            if(pageSelected == perksPages[i].weaponType.ToString())
            {
                currentPerksPageSelected.gameObject.SetActive(false);
                currentPageSelected.color = unselectedColor;

                currentPerksPageSelected = perksPages[i];
                currentPageSelected = pageSelections[i];

                currentPerksPageSelected.gameObject.SetActive(true);
                currentPageSelected.color = selectedColor;
            }
        }
    }
}
