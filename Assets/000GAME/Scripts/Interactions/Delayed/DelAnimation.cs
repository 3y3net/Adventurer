﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DelAnimation : DelayedReactionBase
{

    public Animator animator;   // The Animator that will have its trigger parameter set.
    public string trigger;      // The name of the trigger parameter to be set.


    private int triggerHash;    // The hash representing the trigger parameter to be set.


    protected override void SpecificInit()
    {
        triggerHash = Animator.StringToHash(trigger);
    }


    protected override void ImmediateReaction()
    {
        animator.SetTrigger(triggerHash);
    }
}
