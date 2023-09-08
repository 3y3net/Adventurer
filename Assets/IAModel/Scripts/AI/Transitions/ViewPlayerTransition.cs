using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_3y3net
{
    public class ViewPlayerTransition : BaseTransition
    {

        public bool NegativeCondition = false;
        public float triggerDistance = 100000f;

        public override void OnTransitionTrigger(StateController stController, Collider other, TriggerType type, LayerMask layerMask)
        {

        }

        public override void InitializeTransition(StateController stController)
        {

        }

        public override void OnTransition(StateController stController)
        {

        }

        public override bool TransitionCondition(StateController stController)
        {
            if (stController.triggersDetected.Player != null && stController.triggersDetected.distancePlayer<triggerDistance && !NegativeCondition)
            {
                stController.currentTarget = stController.triggersDetected.lastViewedPlayer;
                return true;
            }
            else if (stController.triggersDetected.Player != null && NegativeCondition)
                return false;

            return NegativeCondition;
        }
    }
        
}