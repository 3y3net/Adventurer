using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour {

    public Transform interactionLocation;                   // The position and rotation the player should go to in order to interact with this Interactable.

    public GameConditionList[] conditonList;

    public ReactionList defaultReactionList;    // If none of the ConditionCollections are reacted to, this one is used.

    public GameObject HighLightObject;
    public Color highLightColor = new Color(180f/255f, 180f / 255f, 180f / 255f,1);

    HighlightingSystem.Highlighter hl;
    public GameCursor gameCursor;

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

    void OnMouseEnter()
    {

    }

    private void OnMouseOver()
    {
        if (hl.mode != HighlightingSystem.HighlighterMode.Disabled)
        {        
            CursorManager.instance.SetCursor(gameCursor);
        }
    }

    private void OnMouseExit()
    {
        CursorManager.instance.SetCursor(GameCursor.ModeNormal);
    }
}
