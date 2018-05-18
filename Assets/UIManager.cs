using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Weapons Vault")]
    public GameObject weaponsVault;
    public KeyCode toggleWeaponsVault;

    private void Update()
    {
        if (Input.GetKeyDown(toggleWeaponsVault))
            weaponsVault.SetActive(!weaponsVault.activeSelf);
    }
}
