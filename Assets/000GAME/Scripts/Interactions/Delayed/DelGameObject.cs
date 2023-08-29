using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DelGameObject : DelayedReactionBase
{

    public GameObject go;       // The gameobject to be turned on or off.
    public string gameObjectName = "";
    public bool activeState;            // The state that the gameobject will be in after the Reaction.
    public bool ReplaceHighlight = false;

    protected override void ImmediateReaction()
    {
        if (go == null && gameObjectName.Length>0)
            go = GameObject.Find(gameObjectName);
        if (go == null)
            return;
        go.SetActive(activeState);
        if(ReplaceHighlight)
        {
            InteractionObject io = GetComponentInParent<InteractionObject>();
            if (io != null)
                io.HighLightObject = go;
        }
    }
}
