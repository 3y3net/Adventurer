using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_3y3net
{
    public class SmellTransition : BaseTransition
    {
        public bool NegativeCondition = false;
        public float triggerDistance = 100000f;

        public override void InitializeTransition(StateController stController)
        {
            
        }

        public override void OnTransition(StateController stController)
        {
            
        }

        public override bool TransitionCondition(StateController stController)
        {
            if (stController.triggersDetected.Smelled != null && stController.triggersDetected.distanceSmell < triggerDistance && !NegativeCondition)
            {
                stController.currentTarget = stController.triggersDetected.lastSmelledObject;
                return true;
            }
            else if (stController.triggersDetected.Smelled != null && NegativeCondition)
                return false;

            return NegativeCondition;
        }
    }
}