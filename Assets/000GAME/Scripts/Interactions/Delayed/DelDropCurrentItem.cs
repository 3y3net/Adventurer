using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelDropCurrentItem : DelayedReactionBase
{

    private DD_GameManager.InventoryManager inventory;    // Reference to the Inventory component.


    protected override void SpecificInit()
    {
        inventory = GameObject.FindObjectOfType<DD_GameManager.InventoryManager>();
    }


    protected override void ImmediateReaction()
    {
        inventory.DeleteSelectedItem();
    }
}