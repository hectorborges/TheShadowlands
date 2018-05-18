using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public Weapon[] weapons;
    public static Weapons instance;

    private void Start()
    {
        instance = this;   
    }
}
