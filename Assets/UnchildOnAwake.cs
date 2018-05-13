using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnchildOnAwake : MonoBehaviour
{
    private void Awake()
    {
        transform.SetParent(null);
    }
}
