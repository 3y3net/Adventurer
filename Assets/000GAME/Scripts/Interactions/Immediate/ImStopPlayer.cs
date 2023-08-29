using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImStopPlayer : ReactionBase
{

    public Transform newDestination;


    protected override void ImmediateReaction()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement)
        {
            if (newDestination != null)
                playerMovement.ForceDestination(newDestination.position);
            else
                playerMovement.ForcePosition(playerMovement.transform);
        }            
        
    }

}
