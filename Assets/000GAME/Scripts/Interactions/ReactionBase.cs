﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ReactionBase:MonoBehaviour {

    // This is called from ReactionCollection.
    // This function contains everything that is required to be done for all
    // Reactions as well as call the SpecificInit of the inheriting Reaction.
    public void Init()
    {
        SpecificInit();
    }


    // This function is virtual so that it can be overridden and used purely
    // for the needs of the inheriting class.
    protected virtual void SpecificInit()
    { }


    // This function is called from ReactionCollection.
    // It contains everything that is required for all for all Reactions as
    // well as the part of the Reaction which needs to happen immediately.
    public void React(MonoBehaviour monoBehaviour)
    {
        ImmediateReaction();
    }


    // This is the core of the Reaction and must be overridden to make things happpen.
    protected abstract void ImmediateReaction();
}
