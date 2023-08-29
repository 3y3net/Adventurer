using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour {

    public enum Activation
	{        
        MouseOver,
        PlayerEnter,
        Both
	}

    public Activation activedWhen;

    public Transform interactionLocation;                   // The position and rotation the player should go to in order to interact with this Interactable.

    public GameConditionList[] conditonList;

    public ReactionList defaultReactionList;    // If none of the ConditionCollections are reacted to, this one is used.

    public GameObject HighLightObject;
    public Color highLightColor = new Color(180f/255f, 180f / 255f, 180f / 255f,1);
    public float flashDuration=0.5f;

    public HighlightingSystem.Highlighter hl;
    public GameCursor gameCursor;

    bool triggered = false;

    void Start()
    {
        hl = HighLightObject.GetComponent<HighlightingSystem.Highlighter>();
    }

    // This is called when the player arrives at the interactionLocation.
    public void Interact()
    {
        // Go through all the ConditionCollections...
        for (int i = 0; i < conditonList.Length; i++)
        {
            // ... then check and potentially react to each.  If the reaction happens, exit the function.
            if (conditonList[i].CheckAndReact())
                return;
        }

        // If none of the reactions happened, use the default ReactionCollection.
        if(defaultReactionList)
            defaultReactionList.React();
    }

    void OnTriggerEnter(Collider other)
    {
        if (activedWhen == Activation.PlayerEnter || activedWhen == Activation.Both)
        {
            if (other.GetComponent<Collider>().tag == "Player" && SceneManager.instance.gameMode == SceneManager.GameMode.Locomotion)
            {
                hl.tween = true;
                hl.tweenDuration = flashDuration;
                if (triggered)
                    return;
                triggered = true;                
            }
        }    
    }

    void OnTriggerExit(Collider other)
    {
        if (activedWhen == Activation.PlayerEnter || activedWhen == Activation.Both)
        {
            if (other.GetComponent<Collider>().tag == "Player" && SceneManager.instance.gameMode == SceneManager.GameMode.Locomotion)
            {                
                triggered = false;
                hl.tween = false;
            }
        }
    }

	private void Update()
	{
        if (triggered)
        {
            Collider col = GetComponent<Collider>();
            if (col != null && col.enabled && Input.GetButtonDown("Action"))
                Interact();
            //hl.Hover(highLightColor);
        }
    }

	void OnMouseEnter()
    {

    }

    public void OnPointerClick(UnityEngine.EventSystems.PointerEventData ped)
    {
        Debug.Log("Clicked");
    }

    private void OnMouseOver()
    {
        if (activedWhen == Activation.MouseOver || activedWhen == Activation.Both)
        {
            hl.Hover(highLightColor);
            CursorManager.instance.SetCursor(gameCursor);
        }
    }

    private void OnMouseExit()
    {
        if (activedWhen == Activation.MouseOver || activedWhen == Activation.Both)
        {            
            CursorManager.instance.SetCursor(GameCursor.ModeNormal);
        }
    }
}
