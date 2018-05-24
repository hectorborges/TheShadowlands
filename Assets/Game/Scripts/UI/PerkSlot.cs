using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PerkSlot : MonoBehaviour
{
    Perk perk;
    Image icon;
    EventTrigger trigger;
    Text nameArea;
    Text descriptionArea;

    private void Start()
    {
        icon = GetComponent<Image>();
        trigger = GetComponent<EventTrigger>();

        Utility.SetTransparency(icon, .5f);
        trigger.enabled = false;
    }

    public void Initialize(Perk _perk, Text _descriptionArea, Text _nameArea)
    {
        if (!icon)
            icon = GetComponent<Image>();

        perk = _perk;
        descriptionArea = _descriptionArea;
        nameArea = _nameArea;

        icon.sprite = perk.perkIcon;
    }

    public void UnlockPerk()
    {
        Utility.SetTransparency(icon, 1);
    }

    public void ViewDescription()
    {
        descriptionArea.transform.parent.gameObject.SetActive(true);
        nameArea.text = perk.perkName;
        descriptionArea.text = perk.perkDescription;
    }

    public void StopViewingDescription()
    {
        nameArea.text = "";
        descriptionArea.text = "";
        descriptionArea.transform.parent.gameObject.SetActive(false);
    }
}
