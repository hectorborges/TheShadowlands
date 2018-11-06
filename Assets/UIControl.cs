using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    public KeyCode mapKey;
    public GameObject map;

    private void Update()
    {
        if (Input.GetKeyDown(mapKey))
            map.SetActive(!map.activeInHierarchy);
    }
}
