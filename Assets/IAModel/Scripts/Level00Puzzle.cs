using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level00Puzzle : LevelPuzzle
{
    public GameObject Smoke1, Smoke2;
    public InteractableInstance ConsoleTower;

    public override void Interact(int interactoinIndex)
    {
        GameLogic.instance.HideInteract();

        switch(interactoinIndex)
        {
            case 0:
                InteractBlueKey();
                break;
        }        
    }

    void InteractBlueKey()
    {
        if (GameLogic.instance.inventoryObject.Contains(DropInstance.DropItem.BlueKey))
        {
            Smoke1.SetActive(false);
            Smoke2.SetActive(false);
            ConsoleTower.IsActive = false;
            GameLogic.instance.ShowInteract("DONE! Now go to extraction point");
        }
        else
        {
            GameLogic.instance.ShowInteract("You need the Blue Key!");
        }
    }
}
