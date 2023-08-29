using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImRepaintGoals : ReactionBase
{
    DD_GameManager.GameGoalsManager gameStateManager;

    protected override void SpecificInit()
    {
        gameStateManager = GameObject.FindObjectOfType<DD_GameManager.GameGoalsManager>();
        if (!gameStateManager)
            throw new UnityException("GameGoalsManager could not be found, ensure that it exists in the Persistent scene.");
    }

    protected override void ImmediateReaction()
    {
        gameStateManager.RepaintItemAllToDo();
    }
}
