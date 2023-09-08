using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI_3y3net
{
    public class HittedTransition : BaseTransition
    {
        public override void InitializeTransition(StateController stController)
        {
            
        }

        public override void OnTransition(StateController stController)
        {
            
        }

        public override bool TransitionCondition(StateController stController)
        {
            if (stController.triggersDetected.hitted && stController.currentState.stateName!=StateName.Pursuit)
            {
                stController.triggersDetected.hitted = false;
                stController.currentTarget = stController.triggersDetected.lastListenedNoise;
                return true;
            }
            
            return false;
        }

    }
}