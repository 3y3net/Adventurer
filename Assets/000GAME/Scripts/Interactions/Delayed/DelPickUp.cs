using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DelPickUp : DelayedReactionBase
{

    public int itemDBIndex;               // The item asset to be added to the Inventory.
    public GameObject ToInventory;

    private DD_GameManager.InventoryManager inventory;    // Reference to the Inventory component.


    protected override void SpecificInit()
    {
        inventory = GameObject.FindObjectOfType<DD_GameManager.InventoryManager>();
    }


    protected override void ImmediateReaction()
    {
        inventory.AddItemToInventory(itemDBIndex);

        if (ToInventory != null)
        {
            DD_GameManager.InventoryItem item = DD_GameManager.InventoryDatabase.instance.items[itemDBIndex];
            GameObject addNotif = Instantiate(ToInventory, ToInventory.transform.parent);
            Image notificatioText = addNotif.GetComponent<Image>();
            notificatioText.sprite = Sprite.Create(item.Icon, new Rect(0, 0, item.Icon.width, item.Icon.height), new Vector2(0.5f, 0.5f));
            addNotif.SetActive(true);
            addNotif.GetComponent<Animator>().SetTrigger("ShowNotification");
        }
    }
}
