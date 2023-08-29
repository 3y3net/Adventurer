using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImChangeCursor : ReactionBase
{

    public GameCursor targetCursor;
    public InteractionObject toChange;    

    protected override void ImmediateReaction()
    {
        toChange.gameCursor = targetCursor;
    }
}