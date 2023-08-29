using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DelLostItem : DelayedReactionBase
{

    public int itemIndex;               // The item asset to be added to the Inventory.


    private DD_GameManager.InventoryManager inventory;    // Reference to the Inventory component.


    protected override void SpecificInit()
    {
        inventory = GameObject.FindObjectOfType<DD_GameManager.InventoryManager>();
    }


    protected override void ImmediateReaction()
    {
        inventory.RemoveItemToInventory(itemIndex);
    }
}
