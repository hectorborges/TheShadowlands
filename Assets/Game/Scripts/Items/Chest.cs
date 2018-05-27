using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    Animator anim;
    bool opened;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenChest()
    {
        if(!opened)
        {
            opened = true;

            anim.SetBool("Open", true);
            LootTable.instance.NewLootTable();
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
