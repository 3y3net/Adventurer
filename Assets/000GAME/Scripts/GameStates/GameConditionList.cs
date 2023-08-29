using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameConditionList {

    public string description;                                          // Description of the ConditionCollection.  This is used purely for identification in the inspector.
    public GameCondition[] requiredConditions = new GameCondition[0];   // The Conditions that need to be met in order for the ReactionCollection to React.
    public ReactionList reactionList;                                   // Reference to the ReactionCollection that will React should all the Conditions be met.
    public bool reactionAllTrue = true; //Execute default reaction list of true or false

    private GameState gs;

    // This is called by the Interactable one at a time for each of its ConditionCollections until one returns true.
    public bool CheckAndReact()
    {
        if (!gs)
        {
            gs = GameObject.FindObjectOfType<GameState>();
            if (!gs)
                throw new UnityException("GameState could not be found, ensure that it exists in the Persistent scene.");
        }
            // Go through all Conditions...
        for (int i = 0; i < requiredConditions.Length; i++) {
            // ... and check them against the AllConditions version of the Condition.  If they don't have the same satisfied flag, return false.
            if (requiredConditions[i].MatchCondition(gs))
            {
                if (reactionList && !reactionAllTrue)
                    reactionList.React();
                //Debug.Log("return true");
                return true;
            }
        }

        // If there is an ReactionCollection assigned, call its React function.
        if (reactionList && reactionAllTrue)
            reactionList.React();

        // A Reaction happened so return true.
        //Debug.Log("return false");
        return false;
    }
}
