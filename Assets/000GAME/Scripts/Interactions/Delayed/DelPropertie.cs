using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DelPropertie : DelayedReactionBase
{
    public enum Property { Collider }
    public GameObject target;
    public Property property;     // The Behaviour to be turned on or off.
    public bool enabledState;       // The state the Behaviour will be in after the Reaction.


    protected override void ImmediateReaction()
    {
        switch(property)
        {
            case Property.Collider:
                if(target!=null && target.GetComponent<Collider>()!=null)
                    target.GetComponent<Collider>().enabled = enabledState;
                break;
        }
        
    }
}
