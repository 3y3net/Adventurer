using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionInventoryItem : MonoBehaviour {

    public GameConditionList[] conditonList;

    public ReactionList defaultReactionList;    // If none of the ConditionCollections are reacted to, this one is used.

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
        if (defaultReactionList)
            defaultReactionList.React();
    }
}
