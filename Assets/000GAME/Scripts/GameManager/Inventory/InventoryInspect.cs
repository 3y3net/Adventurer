using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInspect : MonoBehaviour {

    private DD_GameManager.InventoryManager inventory;    // Reference to the Inventory component.    
    CamManager cm;
    public GameObject dummyVcam;

    [System.Serializable]
    public struct ItemInteraction
    {
        public int itemIndex;
        public InteractionInventoryItem interaction;
        public bool useVirtualVcam;
    }
    public ItemInteraction[] interactions;

    void Start()
    {
        inventory = FindObjectOfType<DD_GameManager.InventoryManager>();
        inventory.OnItemInspect += OnInspect;

        cm = FindObjectOfType<CamManager>();
        if (!cm)
            throw new UnityException("CamManager could not be found, ensure that it exists in the Persistent scene.");
    }
	
	public void OnInspect(int index)
    {       
        //Debug.Log("Inspecting "+inventory.items[index].Name);
        for (int i=0; i<interactions.Length; i++)
            if(interactions[i].itemIndex== inventory.items[index].UID)
            {
                if (interactions[i].useVirtualVcam)
                    cm.AddvCam(dummyVcam, ExitCam, ExitCondition);
                else
                {
                    GameState gameState = FindObjectOfType<GameState>();
                    if (gameState != null) 
                        gameState.gameStates[(int)GameStates.TableVisible] = false;
                }
                //DD_GameManager.GameUIManager.instance.HideInterface();
                DD_GameManager.GameUIManager.instance.FastHide();
                interactions[i].interaction.Interact();
            }
    }

    public void CloseCam()
    {
        //Debug.Log("Fast Restore");
        cm.BackClicked();
        DD_GameManager.GameUIManager.instance.FastRestore();
        Invoke("StopAll", 0.05f);
        //DD_GameManager.GameUIManager.instance.ShowInterface();
        //DD_GameManager.GameUIManager.instance.gameApps[0].FastOpenApp();
    }

    void StopAll()
    {
        PlayerMovement pm = FindObjectOfType<PlayerMovement>();
        if (pm)
            pm.StopAll();
    }

    public void ExitCam()
    {
        
    }

    public bool ExitCondition()
    {
        return true;
    }
}
