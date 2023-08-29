using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelCallEvent : DelayedReactionBase
{

    public UnityEvent invokeEvent;


    protected override void ImmediateReaction()
    {
        //Debug.Log("INVOKE");
        if(invokeEvent!=null)
            invokeEvent.Invoke();
    }
}
