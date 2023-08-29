using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImChangeHighlight : ReactionBase
{
    public GameStates gameState;
    public bool stateCondition;
    public InteractionObject inteactionObject;
    public GameObject newHighLight;

    GameState gs;

    protected override void SpecificInit()
    {
        gs = GameObject.FindObjectOfType<GameState>();
        if (!gs)
            throw new UnityException("GameState could not be found, ensure that it exists in the Persistent scene.");
    }

    protected override void ImmediateReaction()
    {
        if(gs.gameStates[(int)gameState] == stateCondition)
            inteactionObject.HighLightObject = newHighLight;
    }
}
