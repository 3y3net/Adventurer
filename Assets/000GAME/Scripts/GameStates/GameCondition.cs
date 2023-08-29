using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameCondition {
    public enum Compare { none, equals, less, greater };
    public GameStates gameState;
    public bool stateCondition;
    public GameValues gameValue;
    public Compare compairson;
    public int valueToCompare;

    //public ReactionList OnTrue;               // Reference to the ReactionCollection that will React should all the Conditions be met.

    public bool MatchCondition(GameState gs)
    {
        bool match = true;
        if (gs.gameStates[(int)gameState] != stateCondition && gameState!=GameStates.Ignore)
                match = false;

        if(gameValue!=GameValues.None)
            switch (compairson)
            {
                case Compare.none:
                    break;
                case Compare.equals:
                    if (gs.gameValues[(int)gameValue] != valueToCompare)
                        match = false;
                    break;
                case Compare.less:
                    if (gs.gameValues[(int)gameValue] >= valueToCompare)
                        match = false;
                    break;
                case Compare.greater:
                    if (gs.gameValues[(int)gameValue] <= valueToCompare)
                        match = false;
                    break;
            }
        return match;
    }
}
