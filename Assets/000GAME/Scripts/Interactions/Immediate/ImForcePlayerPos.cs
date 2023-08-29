using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImForcePlayerPos : ReactionBase
{

    public PlayerMovement Player;
    public Transform finalPos;

    protected override void ImmediateReaction()
    {
        Player.ForcePosition(finalPos);
    }
}
